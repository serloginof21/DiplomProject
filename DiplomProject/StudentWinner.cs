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
    
    public partial class StudentWinner
    {
        public int Id_Winner { get; set; }
        public int Id_WinnerStudent { get; set; }
        public int Id_WinnerPlace { get; set; }
        public int Id_ChampionStage { get; set; }
        public System.DateTime DateOfWin { get; set; }
    
        public virtual CampionatStages CampionatStages { get; set; }
        public virtual PlaceOfWinners PlaceOfWinners { get; set; }
        public virtual Student Student { get; set; }
    }
}
