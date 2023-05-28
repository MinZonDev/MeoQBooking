namespace WebBooking.Models.DB
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            Bills = new HashSet<Bill>();
        }

        public int bookingid { get; set; }

        public int roomid { get; set; }

        [StringLength(128)]
        public string userid { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng!")]
        [StringLength(100, ErrorMessage = "Vui lòng nhập tên không quá 100 kí tự!")]
        public string customername { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email!")]
        [StringLength(100, ErrorMessage = "Email không được quá 100 kí tự!")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập email đúng định dạng!")]
        public string email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [StringLength(10,ErrorMessage = "Số điện thoại không được vượt quá 10 số!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại không đúng!")]
        public string phone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập CMND/CCCD!")]
        [StringLength(12)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "CMND/CCCD sai định dạng!")]
        public string identiyid { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập ngày sinh!")]
        //[Range(typeof(DateTime), "1/1/1900", "{0:dd/MM/yyyy}", ErrorMessage = "Ngày sinh phải nhỏ hơn ngày hiện tại!")]
        public DateTime? birthday { get; set; }

        public DateTime? bookingdate { get; set; }

        public int? numberpeople { get; set; }
        [GreaterThanCurrentDate(ErrorMessage = "Ngày check-in phải lớn hơn ngày hiện tại!")]
        public DateTime? checkin { get; set; }
        [GreaterThan(nameof(checkin), ErrorMessage = "Ngày check-out phải lớn hơn ngày check-in!")]
        public DateTime? checkout { get; set; }

        [Column(TypeName = "money")]
        public decimal? total { get; set; }

        public int? statusid { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bill> Bills { get; set; }

        public virtual Room Room { get; set; }

        public virtual Status Status { get; set; }
    }
    public class GreaterThanCurrentDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime? checkin = value as DateTime?;

            if (checkin.HasValue)
            {
                DateTime currentDate = DateTime.Now;
                return checkin.Value > currentDate;
            }

            return true; // Trường hợp không có giá trị, vẫn cho phép đi qua kiểm tra khác
        }
    }
    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _dependentPropertyName;

        public GreaterThanAttribute(string dependentPropertyName)
        {
            _dependentPropertyName = dependentPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dependentProperty = validationContext.ObjectType.GetProperty(_dependentPropertyName);

            if (dependentProperty == null)
            {
                return new ValidationResult($"Thuộc tính '{_dependentPropertyName}' không tồn tại trên đối tượng.");
            }

            var dependentPropertyValue = dependentProperty.GetValue(validationContext.ObjectInstance);

            if (value is IComparable && dependentPropertyValue is IComparable)
            {
                if (Comparer.Default.Compare(value, dependentPropertyValue) <= 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }



}
