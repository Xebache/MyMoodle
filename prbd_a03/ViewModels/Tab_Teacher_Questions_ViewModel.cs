using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.ObjectModel;

namespace Moodle.ViewModels {
    public class Tab_Teacher_Questions_ViewModel : Common_ViewModel {

        #region Fields

        public Questions_Edit_ViewModel QuestionsEditViewModel { get; set; } = new Questions_Edit_ViewModel();
        public Questions_List_ViewModel QuestionsListViewModel { get; set; } = new Questions_List_ViewModel();

        private Course _course;
        public Course Course {
            get => _course;
            set => SetProperty(ref _course, value);
        }

        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions {
            get => _questions;
            set => SetProperty(ref _questions, value);
        }

        private ObservableCollection<Category> _categories = new();
        public ObservableCollection<Category> Categories {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        #endregion

        #region Constructor

        public Tab_Teacher_Questions_ViewModel() {
            CommandsManager();
        }

        public void Init(Course course) {
            this.BindOneWay(nameof(Course), QuestionsEditViewModel, nameof(QuestionsEditViewModel.Course));
            this.BindOneWay(nameof(Course), QuestionsListViewModel, nameof(QuestionsListViewModel.Course));
            this.BindOneWay(nameof(Questions), QuestionsListViewModel, nameof(QuestionsListViewModel.Questions));
            this.BindOneWay(nameof(Categories), QuestionsListViewModel, nameof(QuestionsListViewModel.Categories));

            Course = course;
            Questions = Course.QuestionsToEdit();
            Categories = new ObservableCollection<Category>(Course.Categories);
            OnRefreshData();
        }

        #endregion

        #region Command

        private void CommandsManager() {
            QuestionChanged_Register();
            QuestionsUnModifiable_Register();
            NewCategoryAdded_Register();
            QuestionDeleted_Register();
            NewQuestion_Register();
        }

        private void QuestionChanged_Register() {
            Register<Question>(this, AppMessages.MSG_QUESTION_EDITED, question => OnRefreshData());
        }

        private void QuestionsUnModifiable_Register() {
            Register(this, AppMessages.MSG_QUESTIONS_UNMODIFIABLE, OnRefreshData);
        }

        private void NewCategoryAdded_Register() {
            Register(this, AppMessages.MSG_NEW_CATEGORY_ADDED, OnRefreshData);
        }

        private void QuestionDeleted_Register() {
            Register<Question>(this, AppMessages.MSG_QUESTION_DELETED, question => OnRefreshData());
        }

        private void NewQuestion_Register() {
            Register(this, AppMessages.MSG_NEW_QUESTION, OnRefreshData);
        }

        #endregion

        protected override void OnRefreshData() {
            if (Course == null)
                return;

            Course = Course.GetByTitle(Course.Title);

            if (LoggedUser?.Email != null) {
                var user = User.GetByEmail(LoggedUser.Email);
                Login(user);
            }

            Questions = Course.QuestionsToEdit();
            Categories = new ObservableCollection<Category>(Course.Categories);
            RaisePropertyChanged();
        }

    }

}

