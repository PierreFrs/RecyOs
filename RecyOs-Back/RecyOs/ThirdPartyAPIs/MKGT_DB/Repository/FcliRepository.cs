using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RecyOs.MKGT_DB.Entities;
using RecyOs.MKGT_DB.Interfaces;
using NLog;

namespace RecyOs.MKGT_DB.Repository;

public class FcliRepository: BaseMkgtRepository, IFCliRepository<Fcli>
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public FcliRepository(IConfiguration config) : base(config)
    {
    }

    /// <summary>
    /// Parse une ligne de données (DataRow) pour construire et retourner une instance de la classe Fcli.
    /// </summary>
    /// <param name="prmRaw">La ligne de données contenant les informations du client.</param>
    /// <returns>
    /// Une instance de la classe Fcli contenant les informations extraites de la ligne de données.
    /// Si prmRaw est null, retourne null.
    /// </returns>
    /// <remarks>
    /// La méthode s'attend à ce que la DataRow passée en paramètre ait des colonnes spécifiques avec des noms prédéfinis.
    /// Par exemple : CODE, COD_JDE, NOM, ADR1, ADR2, etc. Si ces colonnes n'existent pas, une exception sera levée lors de l'exécution.
    /// </remarks>
    public Fcli ParseDataRow(DataRow prmRaw)
    {
        if (prmRaw == null)
            return null;
        Fcli fcli = new Fcli
        {
            Code = prmRaw["CODE"] as string, // Code MKGT
            Cod_Jde = prmRaw["COD_JDE"] as string,
            Nom = prmRaw["NOM"] as string, // Nom Client
            Adr1 = prmRaw["ADR1"] as string, // Adresse 1 Client
            Adr2 = prmRaw["ADR2"] as string, // Adresse 2 Client
            Adr3 = prmRaw["ADR3"] as string, // Adresse 3 Client
            Cp = prmRaw["CP"] as string, // Code Postal Client
            Ville = prmRaw["VILLE"] as string, // Ville Client
            Cod_Pay = prmRaw["COD_PAY"] as string, // Code Pays Client
            Pays = prmRaw["PAYS"] as string, // Pays Client
            Dvs = prmRaw["DVS"] as string, // Devise Client
            Siret = prmRaw["SIRET"] as string, // Siret Client
            Ape = prmRaw["APE"] as string, // Ape Client
            Intrc = prmRaw["INTRC"] as string, // Intracommunautaire Client
            Intl1 = prmRaw["INTL1"] as string,
            Intl2 = prmRaw["INTL2"] as string,
            Intl3 = prmRaw["INTL3"] as string,
            T1 = prmRaw["T1"] as string, // Telephone 1 Client
            F1 = prmRaw["F1"] as string, // Fax 1 Client
            T2 = prmRaw["T2"] as string, // Telephone 2 Client
            F2 = prmRaw["F2"] as string, // Fax 2 Client
            T3 = prmRaw["T3"] as string, // Telephone 3 Client
            F3 = prmRaw["F3"] as string, // Fax 3 Client
            Email = prmRaw["EMAIL"] as string, // Email Client
            Email1 = prmRaw["EMAIL1"] as string, // Email facturation Client
            Email2 = prmRaw["EMAIL2"] as string, // Email altrenatif Client
            Cc = prmRaw["CC"] as string,
            Site = prmRaw["SITE"] as byte?,
            Secteur = prmRaw["SECTEUR"] as string,
            Sup = prmRaw["SUP"] as char?, // Suppression Client
            Dat_Cre = prmRaw["DAT_CRE"] as DateTime?,
            Dat_Mdf = prmRaw["DAT_MDF"] as DateTime?,
            Obsf = prmRaw["OBSF"] as string, // Observation Facturation Client
            Obse = prmRaw["OBSE"] as string, // Observation Exploitation
            Fr_G = prmRaw["FR_G"] as decimal?,
            Ad2_C = prmRaw["AD2_C"] as string,
            Ad3_C = prmRaw["AD3_C"] as string,
            Cp_C = prmRaw["CP_C"] as string,
            Vil_C = prmRaw["VIL_C"] as string,
            Pay_C = prmRaw["PAY_C"] as string,
            Intl_C = prmRaw["INTL_C"] as string,
            Nom_F = prmRaw["NOM_F"] as string,
            Ad1_F = prmRaw["AD1_F"] as string,
            Ad2_F = prmRaw["AD2_F"] as string,
            Ad3_F = prmRaw["AD3_F"] as string,
            Cp_F = prmRaw["CP_F"] as string,
            Vil_F = prmRaw["VIL_F"] as string,
            Pay_F = prmRaw["PAY_F"] as string,
            Intl_F = prmRaw["INTL_F"] as string,
            Obs_Ec = prmRaw["OBS_EC"] as string,
            Bqe_Def = prmRaw["BQE_DEF"] as string,
            W_Sh = prmRaw["W_SH"] as char?,
            Zn_Cli = prmRaw["ZN_CLI"] as string,
            Aev_Id = prmRaw["AEV_ID"] as string,
            Tva_Lx = prmRaw["TVA_LX"] as string,
            Cl_Rsq = prmRaw["CL_RSQ"] as string,
            Sd_Pes = prmRaw["SD_PES"] as char?,
            Nom_M = prmRaw["NOM_M"] as string,
            Adr1_M = prmRaw["ADR1_M"] as string,
            Adr2_M = prmRaw["ADR2_M"] as string,
            Adr3_M = prmRaw["ADR3_M"] as string,
            Cp_M = prmRaw["CP_M"] as string,
            Vil_M = prmRaw["VIL_M"] as string,
            Tel_M = prmRaw["TEL_M"] as string,
            Fax_M = prmRaw["FAX_M"] as string,
            Intl_M = prmRaw["INTL_M"] as string,
            Email_M = prmRaw["EMAIL_M"] as string,
            Ptb_M = prmRaw["PTB_M"] as string,
            Loc_Tour = prmRaw["LOC_TOUR"] as char?,
            Fact_Min = prmRaw["FACT_MIN"] as decimal?,
            Rfc_Obg = prmRaw["RFC_OBG"] as char?,
            Id_Cliext = prmRaw["ID_CLIEXT"] as string,
            Rfc_Obge = prmRaw["RFC_OBGE"] as char?,
            Grp_Attrb = prmRaw["GRP_ATTRB"] as string,
            Grp_Prm = prmRaw["GRP_PRM"] as string,
            N_Rue = prmRaw["N_RUE"] as string,
            Bis_Ter = prmRaw["BIS_TER"] as string,
            Gps_Lat = prmRaw["GPS_LAT"] as string,
            Gps_Lgt = prmRaw["GPS_LGT"] as string,
            I_Ban = prmRaw["I_BAN"] as string,
            B_Ic = prmRaw["B_IC"] as string,
            Cle_Ext = prmRaw["CLE_EXT"] as int?,
            Tri_Fc = prmRaw["TRI_FC"] as char?,
            Fr_Cd = prmRaw["FR_CD"] as char?,
            Fr_Sr = prmRaw["FR_SR"] as char?,
            Fr_Dc = prmRaw["FR_DC"] as char?,
            Fr_Pv = prmRaw["FR_PV"] as char?,
            Fr_Ta = prmRaw["FR_TA"] as char?,
            Soc_Cs = prmRaw["SOC_CS"] as string,
            Soc_Mq = prmRaw["SOC_MQ"] as string,
            Soc_Autec = prmRaw["SOC_AUTEC"] as char?,
            Cd_Pt1 = prmRaw["CD_PT1"] as string,
            Cd_Pt2 = prmRaw["CD_PT2"] as string,
            Inf_L1 = prmRaw["INF_L1"] as string,
            Inf_L2 = prmRaw["INF_L2"] as string,
            Fr_Cfr = prmRaw["FR_CFR"] as char?,
            Id_Siege = prmRaw["ID_SIEGE"] as char?,
            Abo_Ext = prmRaw["ABO_EXT"] as byte?,
            Inf_1 = prmRaw["INF_1"] as string,
            Inf_2 = prmRaw["INF_2"] as string,
            Inf_3 = prmRaw["INF_3"] as string,
            Inf_4 = prmRaw["INF_4"] as string,
            Inf_5 = prmRaw["INF_5"] as string,
            Pbc_Prv = prmRaw["PBC_PRV"] as char?,
            Mod_Dfs = prmRaw["MOD_DFS"] as char?,
            Anx_Dfs = prmRaw["ANX_DFS"] as string,
            Gid = prmRaw["GID"] as string,
            Ecofolio = prmRaw["ECOFOLIO"] as string,
            I_Gs = prmRaw["I_GS"] as string,
            Fr_Go = prmRaw["FR_GO"] as char?,
            P_Rt = prmRaw["P_RT"] as string,
            Cr_Track = prmRaw["CR_TRACK"] as char?,
            Rcp_Neg = prmRaw["RCP_NEG"] as string,
            Rcpneg_Vd = prmRaw["RCPNEG_VD"] as DateTime?,
            Mode_Reg = prmRaw["MODE_REG"] as string,
            Tva = prmRaw["TAUX_TVA"] as decimal?,
            Cpt_Fac = prmRaw["CPT_FAC"] as string
        };
        return fcli;
    }

    /// <summary>
    /// Récupère une instance de la classe Fcli en fonction du code client spécifié. Seuls les fiches facturation et comunes sont prises en compte.
    /// </summary>
    /// <param name="prmCode">Le code du client à rechercher dans la base de données.</param>
    /// <param name="includeDeleted">Indique si les clients marqués comme supprimés doivent être inclus dans la recherche. Par défaut, ils ne le sont pas.</param>
    /// <returns>
    /// Une tâche représentant l'opération asynchrone. Le résultat de la tâche est une instance de la classe Fcli correspondant au code spécifié.
    /// Si aucun client avec le code spécifié n'est trouvé, retourne null.
    /// </returns>
    /// <remarks>
    /// La méthode effectue une recherche asynchrone dans la table FCLI de la base de données à l'aide du code spécifié.
    /// Elle utilise la méthode <see cref="ParseDataRow"/> pour convertir la ligne de données en une instance de Fcli.
    /// </remarks>
    public async Task<Fcli> GetByCode(string prmCode, bool includeDeleted = false)
    {
        Fcli fcli = null;
        string query = $@"SELECT * FROM FCLI WHERE (FAC_LIV = 'C' OR FAC_LIV = 'F') AND CODE = @prmCode {(includeDeleted ? "" : "AND SUP <> 'O'")};";

        await Task.Run(() =>
        {
            DataTable dataTable = ExecuteQueryWithParameters(query, new SqlParameter("@prmCode", prmCode));

            if (dataTable.Rows.Count > 0)
            {
                DataRow reader = dataTable.Rows[0];
                fcli = this.ParseDataRow(reader);
            }
        });
        return fcli;
    }
    
    /// <summary>
    /// Récupère une liste des instances valides de la classe Fcli depuis la base de données.
    /// </summary>
    /// <returns>
    /// Une tâche représentant l'opération asynchrone. Le résultat de la tâche est une liste des instances de Fcli qui répondent aux critères de validation.
    /// Si aucun client valide n'est trouvé, retourne null.
    /// </returns>
    /// <remarks>
    /// Un client est considéré comme valide si :
    /// - Le champ FAC_LIV vaut 'C' ou 'F'
    /// - Le champ SUP est différent de 'O'
    /// - Le champ SIRET a une longueur de 14, est différent de chaîne vide et ne contient que des chiffres.
    ///
    /// La méthode utilise la méthode <see cref="ParseDataRow"/> pour convertir chaque ligne de données en une instance de Fcli.
    /// </remarks>
    public async Task<IList<Fcli>> GetValidsFcli()
    {
        List<Fcli> fcliList = new List<Fcli>();
        string query = $@"SELECT * FROM FCLI WHERE (FAC_LIV = 'C' OR FAC_LIV = 'F') AND SUP <> 'O' AND (len(SIRET) = 14 AND SIRET <> '' AND PATINDEX('%[^0-9]%', SIRET) = 0);";
        await Task.Run(() =>
        {
            DataTable dataTable = ExecuteQuery(query);

            foreach (DataRow reader in dataTable.Rows)
            {
                fcliList.Add(this.ParseDataRow(reader));
            }
        });

       return fcliList;
    }

    /// <summary>
    /// Appelle la procédure stockée INS_FACT pour insérer ou mettre à jour une instance de la classe Fcli dans la base de données.
    /// </summary>
    /// <param name="prmObj">L'objet Fcli contenant les données à insérer ou à mettre à jour.</param>
    /// <returns>
    /// Une tâche représentant l'opération asynchrone. Si l'opération réussit, le résultat de la tâche est l'objet Fcli entrant (prmObj).
    /// Si une erreur se produit, le résultat est null.
    /// </returns>
    /// <remarks>
    /// Cette méthode assemble un ensemble de paramètres basés sur les propriétés de l'objet Fcli fourni, puis tente d'appeler 
    /// la procédure stockée INS_FACT.
    /// 
    /// Si une exception est levée pendant l'exécution, elle est consignée avec un niveau d'avertissement, et la méthode renvoie null.
    /// </remarks>
    private async Task<Fcli> CallInsFac(Fcli prmObj)
    {
        string procedureName = "INS_FACT";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@CODE", prmObj.Code },
            { "@CPTFAC", prmObj.Cpt_Fac },
            { "@CPTACH", prmObj.Cpt_Ach },
            { "@NOM", prmObj.Nom },
            { "@ADR1", prmObj.Adr1 },
            { "@ADR2", prmObj.Adr2 },
            { "@ADR3", prmObj.Adr3 },
            { "@CP", prmObj.Cp },
            { "@VILLE", prmObj.Ville },
            { "@APE", prmObj.Ape },
            { "@INTRC", prmObj.Intrc },
            { "@SIRET", prmObj.Siret },
            { "@AGENCE", "RECY" },
            { "@SITE", 0 },
            { "@CC", prmObj.Cc },
            { "@F_GP", 'N' },
            { "@NB_CPT", 0 },
            { "@FAM", prmObj.Fam },
            { "@SFAM", "ND" },
            { "@GRP", "" },
            { "@TP_SOC", prmObj.Tp_Soc },
            { "@NUM_CONT", "" },
            { "@RIB", "" },
            { "@MODE_REG", prmObj.Mode_Reg },
            { "@TVA", prmObj.Tva },
            { "@NB_EXP", 1 },
            { "@OBSF", "" },
            { "@INTL1", "" },
            { "@INTL2", prmObj.Intl2 },
            { "@INTL3", prmObj.Intl3 },
            { "@T1", "" },
            { "@T2", prmObj.T2 },
            { "@T3", prmObj.T3 },
            { "@F1", "" },
            { "@F2", "" },
            { "@F3", "" },
            { "@SUP", 'N' },
            { "@MTF", "" },
            { "@DAT_CRE", prmObj.Dat_Cre },
            { "@DAT_MDF", prmObj.Dat_Mdf },
            { "@FL",'F' },
            { "@BANQUE", "" },
            { "@PAYS", prmObj.Pays },
            { "@DVS", "EUR" },
            { "@ENCOURS", prmObj.Encours },
            { "@SECTEUR", prmObj.Secteur },
            { "@SMTVA", prmObj.Sm_Tva },
            { "@FICTRSP", 'N' },
            { "@FICEXUT", 'N' },
            { "@FICITM", 'N' },
            { "@FICSTRT", 'N' },
            { "@EMAIL", "" },
            { "@EMAIL1", prmObj.Email1 },
            { "@EMAIL2", prmObj.Email2 },
            { "@INDC1", "" },
            { "@INDC2", "" },
            { "@INDC3", "" },
            { "@MODREGF", prmObj.Mode_Regf },
            { "@TAUXTVAF", prmObj.Taux_Tvaf },
            { "@PTB1", "" },
            { "@PTB2", prmObj.Ptb2 },
            { "@PTB3", prmObj.Ptb3 },
            { "@CODPAY", prmObj.Cod_Pay },
            { "@MNTCV", DBNull.Value },
            { "@FRNCLI", prmObj.Frn_Cli },
            { "@DEBENC", 'E' },
            { "@TAUXESC", DBNull.Value },
            { "@CPTLEG", "" },
            { "@ENCTIERS", "" },
            { "@FRNCC", DBNull.Value },
            { "@CAPP", DBNull.Value },
            { "@AGP", 'N' },
            { "@NOMC", DBNull.Value },
            { "@AD1C", "" },
            { "@AD2C", "" },
            { "@AD3C", "" },
            { "@CPC", "" },
            { "@VILC", "" },
            { "@PAYC", "" },
            { "@INTLC", "" },
            { "@NOMF", DBNull.Value },
            { "@AD1F", "" },
            { "@AD2F", "" },
            { "@AD3F", "" },
            { "@CPF", "" },
            { "@VILF", "" },
            { "@PAYF", "" },
            { "@INTLF", "" },
            { "@OBSEC", "" },
            { "@BQEDEF", DBNull.Value },
            { "@WSH", DBNull.Value },
            { "@GT", DBNull.Value },
            { "@AEVID", DBNull.Value },
            { "@TVALX", DBNull.Value },
            { "@CLRSQ", DBNull.Value },
            { "@FR_G", DBNull.Value },
            { "@RFCOBG", DBNull.Value },
            { "@RFCOBGE", DBNull.Value },
            { "@ATTRB", DBNull.Value },
            { "@PRM", DBNull.Value },
            { "@NRUE", DBNull.Value },
            { "@BISTER", DBNull.Value },
            { "@TRIFC", "N" },
            { "@SOCCS", "" },
            { "@SOCMQ", "" },
            { "@AUTEC", "N" },
            { "@CDPT1", "" },
            { "@CDPT2", "" },
            { "@VERITCPT", "RY" },
            { "@IDSIEGE", "O" },
            { "@ABOEXT", 0 },
            { "@PBCPRV", "" },
            { "@MODDFS", "I" },
            { "@ANXDFS", "" },
            { "@GID", "" },
            { "@ECOFL", "" },
            { "@RFFC", "" },
            { "@IGS", "" },
            { "@PRT", "" },
            { "@RCPNEG", "" },
            { "@RCPNEGVD", DBNull.Value }
        };

        try
        {
            await ExecuteStoredProcedureAsync(procedureName, parameters);
        }
        catch (Exception e)
        {
            Logger.Warn("Erreur lors de la création d'un facturé : " + e.Message);
            Logger.Warn( "Procédure : " + procedureName);
            Logger.Warn( "Paramètres : " + parameters);
            return null;
        }
        
        return prmObj;
    }

    /// <summary>
    /// Appelle la procédure stockée INS_DT_TIERS pour insérer ou mettre à jour des détails spécifiques de la classe Fcli dans la base de données.
    /// </summary>
    /// <param name="prmObj">L'objet Fcli contenant les données à insérer ou à mettre à jour.</param>
    /// <returns>
    /// Une tâche représentant l'opération asynchrone. Si l'opération réussit, le résultat de la tâche est l'objet Fcli entrant (prmObj).
    /// Si une erreur se produit, le résultat est null.
    /// </returns>
    /// <remarks>
    /// Cette méthode assemble un ensemble de paramètres basés sur les propriétés de l'objet Fcli fourni, puis tente d'appeler 
    /// la procédure stockée INS_DT_TIERS.
    /// 
    /// Si une exception est levée pendant l'exécution, elle est consignée avec un niveau d'avertissement, et la méthode renvoie null.
    /// </remarks>
    private async Task<Fcli> CallInsDt(Fcli prmObj)
    {
        string procedureName = "INS_DT_TIERS";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@CODE", prmObj.Code },
            { "@AG", "RECY"},
            { "@VERITCPT", "RY"}
        };

        try
        {
            await ExecuteStoredProcedureAsync(procedureName, parameters);
        }
        catch (Exception e)
        {
            Logger.Warn("Erreur lors de la création d'un facturé : " + e.Message);
            Logger.Warn( "Procédure : " + procedureName);
            Logger.Warn( "Paramètres : " + parameters);
            return null;
        }

        return prmObj;
    }
    
    /// <summary>
    /// Met à jour un enregistrement spécifique de la table FCLI dans la base de données en utilisant les informations fournies dans l'objet Fcli.
    /// </summary>
    /// <param name="prmObj">L'objet Fcli contenant les données à mettre à jour.</param>
    /// <returns>
    /// Une tâche représentant l'opération asynchrone. Le résultat de la tâche est l'objet Fcli mis à jour.
    /// Si une erreur se produit lors de la mise à jour, le résultat peut être null ou une exception peut être levée.
    /// </returns>
    /// <remarks>
    /// Cette méthode assemble une requête SQL UPDATE pour mettre à jour un enregistrement de la table FCLI.
    /// Les données pour la mise à jour proviennent de l'objet Fcli fourni. La clé primaire pour la mise à jour est la propriété `Code` de l'objet.
    /// </remarks>
    private async Task<Fcli> UpFcli(Fcli prmObj)
    {
                string query = "UPDATE FCLI SET CPT_FAC = @CPTFAC, CPT_ACH = @CPTACH," +
                       " NOM = @NOM," + 
                       " ADR1 = @ADR1," +
                       " ADR2 = @ADR2," +
                       " ADR3 = @ADR3," +
                       " CP = @CP," +
                       " VILLE = @VILLE," +
                       " PAYS = @PAYS," +
                       " APE = @APE," +
                       " INTRC = @INTRC," +
                       " SIRET = @SIRET," +
                       " TP_SOC = @TP_SOC," +
                       " ENCOURS = @ENCOURS," +
                       " MODE_REG = @MODE_REG," +
                       " MODE_REGF = @MODREGF," +
                       " TAUX_TVA = @TVA," +
                       " TAUX_TVAF = @TAUXTVAF," +
                       " INTL2 = @INTL2," +
                       " INTL3 = @INTL3," +
                       " T2 = @T2," +
                       " T3 = @T3," +
                       " DAT_CRE = @DAT_CRE," +
                       " DAT_MDF = @DAT_MDF," +
                       " SM_TVA = @SMTVA," +
                       " EMAIL1 = @EMAIL1," +
                       " EMAIL2 = @EMAIL2," +
                       " PTB2 = @PTB2," +
                       " PTB3 = @PTB3," +
                       " COD_PAY = @CODPAY," +
                       " SECTEUR = @SECTEUR," +
                       " FRN_CLI = @FRNCLI," +
                       " CC = @CC," +
                       " FAM = @FAM" +
                       " WHERE CODE = @CODE";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@CODE", prmObj.Code },
            { "@CPTFAC", prmObj.Cpt_Fac },
            { "@CPTACH", prmObj.Cpt_Ach},
            { "@NOM", prmObj.Nom },
            { "@ADR1", prmObj.Adr1 },
            { "@ADR2", prmObj.Adr2 },
            { "@ADR3", prmObj.Adr3 },
            { "@CP", prmObj.Cp },
            { "@VILLE", prmObj.Ville },
            { "@APE", prmObj.Ape },
            { "@INTRC", prmObj.Intrc },
            { "@SIRET", prmObj.Siret },
            { "@TP_SOC", prmObj.Tp_Soc },
            { "@MODE_REG", prmObj.Mode_Reg },
            { "@MODREGF", prmObj.Mode_Regf},
            { "@TVA", prmObj.Tva },
            { "@TAUXTVAF", prmObj.Taux_Tvaf},
            { "@INTL2", prmObj.Intl2 },
            { "@INTL3", prmObj.Intl3 },
            { "@T2", prmObj.T2 },
            { "@T3", prmObj.T3 },
            { "@DAT_CRE", prmObj.Dat_Cre },
            { "@DAT_MDF", prmObj.Dat_Mdf },
            { "@PAYS", prmObj.Pays },
            { "@ENCOURS", prmObj.Encours },
            { "@SMTVA", prmObj.Sm_Tva },
            { "@EMAIL1", prmObj.Email1 },
            { "@EMAIL2", prmObj.Email2 },
            { "@PTB2", prmObj.Ptb2 },
            { "@PTB3", prmObj.Ptb3 },
            { "@CODPAY", prmObj.Cod_Pay },
            { "@SECTEUR", prmObj.Secteur },
            { "@FRNCLI", prmObj.Frn_Cli },
            { "@CC", prmObj.Cc },
            { "@FAM", prmObj.Fam }
        };
        
        try
        {
            await ExecuteNonQueryWithParametersAsync(query, parameters);
        }
        catch (Exception e)
        {
            Logger.Warn("Erreur lors de la mise à jour d'un facturé exception : " + e.Message);
            Logger.Warn( "Requette : " + query);
            return null;
        }
        
        return prmObj;
    }
    
    /// <summary>
    /// Met à jour un enregistrement spécifique de la table FCLI_DT dans la base de données en utilisant les informations fournies dans l'objet Fcli.
    /// </summary>
    /// <param name="prmObj">L'objet Fcli contenant les données à mettre à jour.</param>
    /// <returns>
    /// Une tâche représentant l'opération asynchrone. Le résultat de la tâche est l'objet Fcli mis à jour.
    /// Si une erreur se produit lors de la mise à jour, le résultat peut être null.
    /// </returns>
    /// <remarks>
    /// Cette méthode construit une requête SQL UPDATE pour mettre à jour un enregistrement de la table FCLI_DT.
    /// Les données pour la mise à jour proviennent de l'objet Fcli fourni. Les critères de mise à jour sont basés sur la propriété `Code` de l'objet et sur une valeur fixe pour le champ `AG` ("RECY").
    /// </remarks>
     private async Task<Fcli> UpFcliDt(Fcli prmObj)
    {
                string query = "UPDATE FCLI_DT SET"+
                               " NOM = @NOM," +
                               " CPT_FAC = @CPTFAC," +
                               " CPT_ACH = @CPTACH," +
                               " TAUX_TVA = @TVA," +
                               " TAUX_TVAF = @TVAF," +
                               " MODE_REG = @MODE_REG," +
                               " MODE_REGF = @MODE_REGF," +
                               " SM_TVA = @SMTVA," +
                               " COD_PAY = @CODPAY," +
                               " ENCOURS = @ENCOURS" +
                               " WHERE CODE =  @CODE AND AG = @AG";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@CODE", prmObj.Code },
            { "@CPTFAC", prmObj.Cpt_Fac },
            { "@CPTACH", prmObj.Cpt_Ach},
            { "@NOM", prmObj.Nom },
            { "@MODE_REG", prmObj.Mode_Reg },
            { "@MODE_REGF", prmObj.Mode_Regf},
            { "@TVA", prmObj.Tva },
            { "@TVAF", prmObj.Taux_Tvaf },
            { "@SMTVA", prmObj.Sm_Tva },
            { "@CODPAY", prmObj.Cod_Pay },
            { "@ENCOURS", prmObj.Encours },
            { "@AG", "RECY" },
        };
        
        try
        {
            await ExecuteNonQueryWithParametersAsync(query, parameters);
        }
        catch (Exception e)
        {
            Logger.Warn("Erreur lors de la mise à jour d'un facturé exception : " + e.Message);
            Logger.Warn( "Requette : " + query);
            return null;
        }
        
        return prmObj;
    }

    /// <summary>
    /// Met à jour le commercial, le siret, l'APE, l'intracomm sur tous les chantiers du client.
    /// </summary>
    /// <param name="prmObj"></param>
    private async Task UpExploit(Fcli prmObj)
    {
        string query = "UPDATE FCLI SET CPT_FAC = @CPTFAC, CC = @CC, SIRET = @SIRET, APE = @APE, INTRC = @INTRC WHERE CODF = @CODE";
        Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@CPTFAC", prmObj.Cpt_Fac },
            { "@CODE", prmObj.Code },
            { "@CC", prmObj.Cc },
            { "@SIRET", prmObj.Siret },
            { "@APE", prmObj.Ape },
            { "@INTRC", prmObj.Intrc }
        };
        try
        {
            await ExecuteNonQueryWithParametersAsync(query, parameters);
        }
        catch (Exception e)
        {
            Logger.Warn("Erreur lors de la mise à jour d'un facturé exception : " + e.Message);
            Logger.Warn( "Requette : " + query);
        }
    }
     
     /* Cette méthode permet de mettre à jour un facturé
 * 
 */
     public async Task<Fcli> UpFac(Fcli prmObj)
     {
         await UpFcliDt(prmObj);
         await UpExploit(prmObj);
         return await UpFcli(prmObj);
     }
     
     /* Cette méthode permet de créer un facturé
     */
    public async Task<Fcli> CreFac(Fcli prmObj)
    {
        await CallInsFac(prmObj);
        await UpExploit(prmObj);
        return await CallInsDt(prmObj);
    }
}