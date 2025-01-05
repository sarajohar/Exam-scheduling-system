using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class EditInvigilatorViewModel
{
    public int ReservationId { get; set; }
    public string CurrentInvigilator { get; set; }

    [Required (ErrorMessage = "Please select an invigilator.")]
    public string NewInvigilator { get; set; }

    public List<string> AvailableInvigilators { get; set; }
}
