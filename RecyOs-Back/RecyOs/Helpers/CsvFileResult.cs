using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVCExport
{
    /// <summary>
    /// Classe permettant de générer un résultat de fichier CSV à partir d'une source de données.
    /// </summary>
    /// <typeparam name="TEntity">Type de l'entité à exporter en CSV.</typeparam>
    public class CsvFileResult<TEntity> : FileResult where TEntity : class
    {
        // Valeurs par défaut
        private const string DefaultContentType = "text/csv";
        private string _delimiter;
        private string _lineBreak;
        private Encoding _contentEncoding;
        private IEnumerable<string> _headers;
        private IEnumerable<PropertyInfo> _sourceProperties;
        private readonly IEnumerable<TEntity> _dataSource;
        private Func<TEntity, IEnumerable<string>> _map;
        
        // Constructeur principal
        public CsvFileResult(IEnumerable<TEntity> source, string fileDownloadName, string contentType = DefaultContentType)
            : base(contentType ?? DefaultContentType)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            _dataSource = source;

            if (string.IsNullOrEmpty(fileDownloadName))
                throw new ArgumentNullException(nameof(fileDownloadName));
            FileDownloadName = fileDownloadName;
            HasPreamble = true; // Par défaut, émet le préambule UTF8
        }

        // Constructeur avec mappage personnalisé et en-têtes
        public CsvFileResult(IEnumerable<TEntity> source, string fileDownloadName, Func<TEntity, IEnumerable<string>> map, IEnumerable<string> headers)
            : this(source, fileDownloadName, DefaultContentType)
        {
            _headers = headers;
            _map = map;
        }

        // Fonction de mappage personnalisée pour convertir une entité en une ligne CSV
        public Func<TEntity, IEnumerable<string>> Map
        {
            get { return _map; }
            set { _map = value; }
        }

        // Source de données à exporter
        public IEnumerable<TEntity> DataSource => _dataSource;

        // Séparateur de colonnes CSV, avec une valeur par défaut basée sur la culture courante
        public string Delimiter
        {
            get
            {
                if (string.IsNullOrEmpty(_delimiter))
                {
                    _delimiter = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                }
                return _delimiter;
            }
            set { _delimiter = value; }
        }

        // Encodage du contenu du fichier CSV
        public Encoding ContentEncoding
        {
            get
            {
                if (_contentEncoding == null)
                {
                    _contentEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: HasPreamble);
                }
                return _contentEncoding;
            }
            set { _contentEncoding = value; }
        }

        // En-têtes des colonnes CSV
        public IEnumerable<string> Headers
        {
            get
            {
                if (_headers == null)
                {
                    _headers = typeof(TEntity).GetProperties().Select(x => x.Name);
                }
                return _headers;
            }
            set { _headers = value; }
        }

        // Propriétés de l'entité à exporter
        public IEnumerable<PropertyInfo> SourceProperties
        {
            get
            {
                if (_sourceProperties == null)
                {
                    _sourceProperties = typeof(TEntity).GetProperties();
                }
                return _sourceProperties;
            }
        }

        // Indique si un préambule UTF8 doit être écrit
        public bool HasPreamble { get; set; }

        // Saut de ligne à utiliser dans le fichier CSV
        public string LineBreak
        {
            get
            {
                if (string.IsNullOrEmpty(_lineBreak))
                {
                    _lineBreak = Environment.NewLine;
                }
                return _lineBreak;
            }
            set { _lineBreak = value; }
        }

        

        // Méthode d'exécution asynchrone pour envoyer le fichier CSV au client
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = ContentType;
            ContentDispositionHeaderValue contentDisposition = new ContentDispositionHeaderValue("attachment");
            contentDisposition.SetHttpFileName(FileDownloadName);
            response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            if (ContentEncoding != null)
            {
                response.Headers.ContentEncoding = new[] { ContentEncoding.WebName };
            }

            var csvData = GetCSVData();
            var streamBuffer = ContentEncoding.GetBytes(csvData);

            if (HasPreamble)
            {
                var preamble = ContentEncoding.GetPreamble();
                await response.Body.WriteAsync(preamble, 0, preamble.Length);
            }

            await response.Body.WriteAsync(streamBuffer, 0, streamBuffer.Length);
        }

        // Génère l'en-tête CSV
        private string GetCSVHeader()
        {
            return string.Join(Delimiter, Headers.Select(FormatCSV));
        }

        // Génère les données CSV
        private string GetCSVData()
        {
            var csv = GetCSVHeader();
            Func<TEntity, string> expr = x => Map == null ? FormatPropertiesCSV(x) : FormatMapCSV(x);
            csv += LineBreak + string.Join(LineBreak, DataSource.Select(expr));
            return csv + LineBreak;
        }

        // Formate une chaîne pour l'export CSV
        private string FormatCSV(string str)
        {
            return str;
        }

        // Formate les propriétés d'une entité pour l'export CSV
        private string FormatPropertiesCSV(TEntity obj)
        {
            var csv = SourceProperties.Select(pi => FormatCSV(GetPropertyValue(pi, obj)));
            return string.Join(Delimiter, csv);
        }

        // Obtient la valeur d'une propriété
        private string GetPropertyValue(PropertyInfo pi, object source)
        {
            try
            {
                var result = pi.GetValue(source, null);
                return result?.ToString() ?? "";
            }
            catch
            {
                return "Can not obtain the value";
            }
        }

        // Formate une entité en utilisant la fonction de mappage personnalisée
        private string FormatMapCSV(TEntity obj)
        {
            return string.Join(Delimiter, Map(obj).Select(FormatCSV));
        }
    }
}
