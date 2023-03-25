using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokenScanner.Models
{
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("symbol")]
        public string Symbol { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("total_supply")]
        public int TotalSupply { get; set; }
        [Column("contract_address")]
        public string ContractAddress { get; set; }
        [Column("total_holders")]
        public int TotalHolders { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
    }
}
