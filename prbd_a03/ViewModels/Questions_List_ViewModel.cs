using Moodle.Models;
using PRBD_Framework;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Moodle.ViewModels {

    public class Questions_List_ViewModel : Common_ViewModel {

        #region Fields

        private Course _course;
        public Course Course {
            get => _course;
            set => SetProperty(ref _course, value, OnRefreshData);
        }

        private ObservableCollection<Category> _categories = new();
        public ObservableCollection<Category> Categories {
            get => _categories;
            set => SetProperty(ref _categories, value, OnRefreshData);
        }

        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions {
            get => _questions;
            set {
                SetProperty(ref _questions, value, OnRefreshData);
                if (_questions != null) {
                    NbAvailable = Questions.Count;
                    NbSelected = FilteredQuestions.Count;
                }
            }
        }

        private ObservableCollection<Question> _filteredQuestions = new();
        public ObservableCollection<Question> FilteredQuestions {
            get => _filteredQuestions;
            set => SetProperty(ref _filteredQuestions, value);
        }

        private Question _selectedItem = new();
        public Question SelectedItem {
            get => _selectedItem;
            set {
                SetProperty(ref _selectedItem, value);
                NotifyColleagues(AppMessages.MSG_DISPLAY_QUESTION_TEACHER, SelectedItem);
            }
        }

        private int _nbSelected;
        public int NbSelected { get => _nbSelected; set => SetProperty(ref _nbSelected, value); }

        private int _nbAvailable;
        public int NbAvailable { get => _nbAvailable; set => SetProperty(ref _nbAvailable, value); }

        public ICommand CategorySelected { get; set; }

        #endregion

        #region Constructor

        public Questions_List_ViewModel() {
            CommandsManager();
        }

        public void Init(Course course) {
            Course = course;
            
        }

        #endregion

        #region Commands

        private void CommandsManager() {
            CategorySelectedCommand();
            QuestionsModified_Register();
            }

            private void CategorySelectedCommand() {
            CategorySelected = new RelayCommand(OnRefreshData);
        }

        private void QuestionsModified_Register() {
            Register(this, AppMessages.MSG_QUESTIONS_LIST_MODIFIED, OnRefreshData);
        }

        #endregion

        private ObservableCollection<Question> Filtered() {
            var QuestionsFiltered = Questions.Where(q => q.Categories.Any(c => c.IsSelected));
            return new ObservableCollection<Question>(QuestionsFiltered);
        }


        protected override void OnRefreshData() {
            if (Questions == null || Course == null || Categories == null)
                return;

            Course = Course.GetByTitle(Course.Title);

            FilteredQuestions = !Categories.Where(c => c.IsSelected).Any() ? Questions : Filtered();
            NbAvailable = Questions.Count;
            NbSelected = FilteredQuestions.Count;

            RaisePropertyChanged();
        }

    }

}
