using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models {

    public class Category : EntityBase<Context> {

        #region Fields

        [Key]
        public int CategoryID { get; set; }
        public String Title { get; set; }

        private bool _isSelected;
        public bool IsSelected {
            get => _isSelected;
            set {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        [Required]
        public virtual Course Course { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public virtual ICollection<Question> Questions { get; set; } = new HashSet<Question>();

        #endregion

        #region Constructors

        public Category(string title, Course course) {
            Title = title;
            Course = course;
            course?.Categories.Add(this);
        }

        public Category() {
        }

        #endregion

    }

}