using DAL.Entity;

namespace DAL.Model
{
    public class UserFilter
    {
        public long? ProjectId { get; set; }

        public UserFilter()
        {
            this.ProjectId = null;
        }

    }
}
