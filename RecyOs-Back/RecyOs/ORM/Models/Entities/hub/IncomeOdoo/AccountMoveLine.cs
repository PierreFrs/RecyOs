using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Models.Entities.hub.IncomeOdoo
{
    [Table("odoo_account_move_line")]
    public class AccountMoveLine
    {
/// <summary>
        /// Identifiant de l'écriture comptable fourni par Odoo.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // L'ID provient d'Odoo, la DB ne le génère pas
        public long Id { get; set; }

        /// <summary>
        /// Référence du mouvement comptable (lié à account.move)
        /// </summary>
        public long MoveId { get; set; }

        /// <summary>
        /// Compte comptable lié à l'écriture comptable
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Nom ou référence du mouvement (ex. numéro d’écriture ou de document)
        /// </summary>
        public string MoveName { get; set; }

        /// <summary>
        /// Date de l’écriture comptable
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Identifiant du partenaire (client, fournisseur, etc.)
        /// </summary>
        public long PartnerId { get; set; }

        /// <summary>
        /// Montant débité (ex. montant facturé)
        /// </summary>
        public decimal? Debit { get; set; }

        /// <summary>
        /// Montant crédité (ex. paiement reçu ou acompte)
        /// </summary>
        public decimal? Credit { get; set; }

        /// <summary>
        /// Solde de l’écriture comptable (différence entre débit et crédit)
        /// </summary>
        public decimal? Balance { get; set; }

        /// <summary>
        /// Date d’échéance, si applicable (ex. date limite de paiement)
        /// </summary>
        public DateTime? DateMaturity { get; set; }

        /// <summary>
        /// reconciled
        /// </summary>
        public bool? Reconciled { get; set; }
    }
}