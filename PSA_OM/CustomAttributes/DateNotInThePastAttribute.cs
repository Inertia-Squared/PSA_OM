using System.ComponentModel.DataAnnotations;

namespace PSA_OM.CustomAttributes
{
    public class DateNotInThePastAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var date = (DateTime)value;
            return date >= DateTime.Now.Date;
        }
    }
}
