using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Web.Infrastructure
{
    public class NonEmptyListAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool isValid = false;
            IEnumerable list = value as IEnumerable;
            if (list == null)
            {
                return true;
            }
            //TODO
            foreach (var item in list)
            {
                isValid = true;
                break;                
            }
            return isValid;
        }
    }
}