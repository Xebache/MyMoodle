using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using Moodle.Utils;

namespace Moodle.ViewModels {

    public class Questions_Edit_ViewModel : Common_ViewModel {

        #region CheckedCategory

        public class CheckedCategory : EntityBase<Context> {

            public Category Category { get; set; }

            private bool _isChecked;
            public bool IsChecked {
                get => _isChecked;
                set {
                    _isChecked = value;
                    RaisePropertyChanged(nameof(IsChecked));
                }
            }

            public CheckedCategory(Category category) {
                Category = category;
                IsChecked = false;
            }

        }

        #endregion

        #region Fields

        private Course _course;
        public Course Course {
            get => _course;
            set => SetProperty(ref _course, value, OnRefreshData);
        }

        private ObservableCollection<CheckedCategory> _checkedCategories = new();
        public ObservableCollection<CheckedCategory> CheckedCategories {
            get => _checkedCategories;
            set => SetProperty(ref _checkedCategories, value);
        }

        private ObservableCollection<Category> _categories = new();
        public ObservableCollection<Category> Categories {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        private Question _question;
        public Question Question {
            get => _question;
            set => SetProperty(ref _question, value, OnRefreshData);
        }

        private string _body;
        public string Body {
            get => _body;
            set {
                ClearErrors();
                SetProperty<string>(ref _body, value);
            }
        }

        private string _answers;
        public string Answers {
            get => _answers;
            set {
                ClearErrors();
                SetProperty(ref _answers, value);
            }
        }

        private bool _multipleAnswers;
        public bool MultipleAnswers {
            get => _multipleAnswers;
            set => SetProperty(ref _multipleAnswers, value);
        }


        public bool SimpleAnswer {
            get => !_multipleAnswers;
            set => SetProperty(ref _multipleAnswers, !value);
        }

        private string _category = "Ajouter une catégorie";
        public string Category { 
            get => _category; set => 
                SetProperty(ref _category, value); 
        }

        private bool _isNew = false;
        public bool IsNew {
            get => _isNew;
            set => SetProperty(ref _isNew, value);
        }

        private bool _isEnabled = false;
        public bool IsEnabled { 
            get => _isEnabled; 
            set => SetProperty(ref _isEnabled, value); 
        }

        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }
        public ICommand NewQuestion { get; set; }
        public ICommand AddCategory { get; set; }

        #endregion

        #region Constructor

        public Questions_Edit_ViewModel() {
            CommandsManager();
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            QuestionSelected_Register();
            SaveCommand();
            CancelCommand();
            DeleteCommand();
            NewQuestionCommand();
            AddCategoryCommand();
        }

        private void AddCategoryCommand() {
            AddCategory = new RelayCommand(AddCategoryAction, CanAddCategory);
        }

        private void QuestionSelected_Register() {
            Register<Question>(this, AppMessages.MSG_DISPLAY_QUESTION_TEACHER, question => {
                Question = question;
                IsNew = false;
                RaisePropertyChanged(nameof(Question));
            });
        }

        private void SaveCommand() {
            Save = new RelayCommand(SaveAction, CanSave);
        }

        private void CancelCommand() {
            Cancel = new RelayCommand(CancelAction, CanCancel);
        }

        private void DeleteCommand() {
            Delete = new RelayCommand(DeleteAction, CanDelete);
        }

        private void NewQuestionCommand() {
            NewQuestion = new RelayCommand(() => {
                Question = new();
                Question.Course = Course;
                IsNew = true;
            });
        }

        #endregion

        #region Actions

        private void SaveAction() {
            if (Validate()) {
                Question.Body = Body;
                Question.MultipleAnswers = MultipleAnswers;
                Question.Choices = StringToChoices();
                ChangeQuestionCategories();
                if(IsNew) {
                    Context.Add(Question);
                    NotifyColleagues(AppMessages.MSG_NEW_QUESTION);
                }
                Context.SaveChanges();
                NotifyColleagues(AppMessages.MSG_QUESTION_EDITED, Question);
                IsNew = false;
                DisableEdit();
                RaisePropertyChanged();
            }
        }

        private bool CanSave() {
            if (IsNew)
                return !string.IsNullOrEmpty(Body) && !string.IsNullOrEmpty(Answers);
            return Question != null && (Body != Question.Body || Answers != Question.ChoicesToString()) && !string.IsNullOrEmpty(Body) && !string.IsNullOrEmpty(Answers);
        }

        private void AddCategoryAction() {
            Category category = new(Category, Course);
            //Question.Categories.Add(category);
            Context.Categories.Add(category);
            //Context.Questions.Update(Question);
            Context.SaveChanges();
            Category = "Ajouter une catégorie";
            NotifyColleagues(AppMessages.MSG_NEW_CATEGORY_ADDED);
            OnRefreshData();
        }

        private bool CanAddCategory() {
            return Category is not "" and not "Ajouter une catégorie" 
                && !Course.Categories.Any(c => c.Title.Equals(Category, StringComparison.Ordinal));
        }

        private void CancelAction() {
            RefreshQuestion();
        }

        private bool CanCancel() {
            return Question != null;
        }

        private bool CanDelete() {
            if(!IsNew)
                return Question != null;
            return false;
        }

        private void DeleteAction() {
            Context.Questions.Remove(Question);
            Context.SaveChanges();
            NotifyColleagues(AppMessages.MSG_QUESTION_DELETED, Question);
            DisableEdit();
        }

        #endregion

        #region Choices To String

        private ICollection<Choice> StringToChoices() {
            ICollection<Choice> choices = new HashSet<Choice>();
            Choice choice = null;

            string[] subs = Answers.Split('\n');

            for(int i = 0; i < subs.Length; ++i) {
                subs[i] = subs[i].Trim();
                if (subs[i].StartsWith('*'))
                    choice = new(subs[i][1..], true, Question);
                else if (subs[i] != "")
                    choice = new(subs[i], false, Question);
                choices.Add(choice);
            }
            return choices;   
        }

        private bool ValidateAnswers(string[] subs) {
            int cpt = 0;
            foreach (var _ in subs.Where(str => str.StartsWith('*')).Select(str => new { })) {
                ++cpt;
            }
            return MultipleAnswers ? cpt > 1 : cpt == 1;
        }

        #endregion

        #region Categories Selection

        private void BuildCheckedCategories() {
            CheckedCategories = new();
            foreach (Category c in Categories) {
                CheckedCategory cc = new(c);
                if (!CheckedCategories.Contains(cc)) {
                    CheckedCategories.Add(cc);
                }
            }
        }

        private void CheckCheckedCategories() {
            foreach (CheckedCategory cc in Question.Categories.SelectMany(c => CheckedCategories.Where(cc => c == cc.Category))) {
                cc.IsChecked = true;
            }
            RaisePropertyChanged(nameof(CheckedCategories));
        }

        private void ChangeQuestionCategories() {
            if (Question != null) {
                foreach (CheckedCategory checkedCategory in CheckedCategories
                    .Where(checkedCategory => checkedCategory.IsChecked && !Question.Categories.Contains(checkedCategory.Category))) {
                    Question.Categories.Add(checkedCategory.Category);
                }
                foreach (CheckedCategory checkedCategory in CheckedCategories
                    .Where(checkedCategory => !checkedCategory.IsChecked && Question.Categories.Contains(checkedCategory.Category))) {
                    Question.Categories.Remove(checkedCategory.Category);
                }
            }
        }

        private void RefreshQuestion() {
            Body = Question.Body;
            Answers = Question.ChoicesToString();
            MultipleAnswers = Question.MultipleAnswers;
            SimpleAnswer = !Question.MultipleAnswers;
            BuildCheckedCategories();
            CheckCheckedCategories();
            RaisePropertyChanged();
        }

        private void DisableEdit() {
            Body = "";
            Answers = "";
            MultipleAnswers = false;
            IsEnabled = false;
            RaisePropertyChanged();
        }

        #endregion

        #region Validation

        public override bool Validate() {
            ClearErrors();

            if (!Validator.IsStringValid(Body)) {
                AddError(nameof(Body), "Titre invalide");
            }
            else if (Answers != null && !ValidateAnswers(Answers.Split('\n'))) {
                AddError(nameof(Answers), "Type de réponse erroné");
            }

            RaiseErrors();
            return !HasErrors;
        }

        #endregion

        protected override void OnRefreshData() {
            if (Course == null)
                return;

            //??????????????????????
            Course = Course.GetByTitle(Course.Title);
            Console.WriteLine(Course.Title);

            Categories = new ObservableCollection<Category>(Course.Categories);
            BuildCheckedCategories();
            RaisePropertyChanged();

            if (Question != null) {
                RefreshQuestion();
                _isEnabled = true;
                RaisePropertyChanged(nameof(IsEnabled));
            }

        }

    }

}
