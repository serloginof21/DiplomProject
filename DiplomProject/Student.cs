//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DiplomProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.StudentWinner = new HashSet<StudentWinner>();
        }
    
        public int Id_Student { get; set; }
        public string SurnameStudent { get; set; }
        public string NameStudent { get; set; }
        public string PatronymicStudent { get; set; }
        public string EmailStudent { get; set; }
        public string PhoneNumberStudent { get; set; }
        public string RegionStudent { get; set; }
        public string CountryStudent { get; set; }
        public int Id_Organization { get; set; }
        public int Id_ClothingSizeStudent { get; set; }
        public int Id_Category { get; set; }
        public int Id_Competence { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual ClothingSizes ClothingSizes { get; set; }
        public virtual Competences Competences { get; set; }
        public virtual Organizations Organizations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentWinner> StudentWinner { get; set; }

        public string FullNameStudent
        {
            get { return $"{SurnameStudent} {NameStudent} {PatronymicStudent}"; }
        }
    }
}
