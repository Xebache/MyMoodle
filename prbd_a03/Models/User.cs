using PRBD_Framework;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Moodle.Models {

    public abstract class User : EntityBase<Context> {

        #region Fields

        [Key]
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        #endregion

        #region Constructors

        protected User(string fullName, string email, string password) {
            FullName = fullName;
            Email = email;
            Password = password;
        }

        public User() { }

        #endregion

        #region Methods

        public override string ToString() {
            return FullName;
        }

        public static User GetByEmail(string email) => Context.Users.SingleOrDefault(u => u.Email.Equals(email));

        #endregion

    }

}
