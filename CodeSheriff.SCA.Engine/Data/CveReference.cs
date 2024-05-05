using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSheriff.SCA.Engine.Data;

public class CveReference
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string CveId { get; set; }

    public string Source { get; set; }
    public string Url { get; set; }

    public CveInfo CveInfo { get; set; }
}
