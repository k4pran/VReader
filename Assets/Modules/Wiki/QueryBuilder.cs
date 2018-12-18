using System;

namespace WikiQueries{
    
    // todo - multiple calls to Query prepends extra '?' due to bug - clear multiple ?s
    
    public class QueryBuilder{
        
        ///-------------------------------------///
        ///             Properties              ///
        ///-------------------------------------///

        private UriBuilder uriBuilder;
        private Format format;
        
        ///-------------------------------------///
        ///             Constructors            ///
        ///-------------------------------------///
        
        public QueryBuilder(string baseUri){
            Uri uri;
            if (!Uri.TryCreate(baseUri, UriKind.Absolute, out uri)){
                throw new InvalidUriException("Base uri " + uri + " is invalid.");
            }
            
            uriBuilder = new UriBuilder(baseUri);
            if (uriBuilder.Query.Length == 0){
                uriBuilder.Query = "";
            }
            format = Format.XML;
        }
        
        public QueryBuilder(string baseUri, Format format){
            Uri uri;
            if (!Uri.TryCreate(baseUri, UriKind.Absolute, out uri)){
                throw new InvalidUriException("Base uri " + uri + " is invalid.");
            }
            
            uriBuilder = new UriBuilder(baseUri);
            if (uriBuilder.Query.Length == 0){
                uriBuilder.Query = "";
            }
            this.format = format;
        }
        
        ///-------------------------------------///
        ///             Methods                 ///
        ///-------------------------------------///

        /// <summary>
        /// Append parameter with single or no value
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <param name="value">Parameter value - optional</param>
        public void AppendParam(string key, string value=""){
            
            if (value.Length == 0){
                uriBuilder.Query += uriBuilder.Query == "?" ? 
                    Uri.EscapeDataString(key) : "&" + Uri.EscapeDataString(key);
                return;
            }
            uriBuilder.Query += uriBuilder.Query == "?" ? "" : "&";
            uriBuilder.Query += Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(value);
        }

        /// <summary>
        /// Append parameter with one or more values
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <param name="values">Parameter values - multiple values seperated by wikis preferred '|' char</param>
        public void AppendMultiValParam(string key, string[] values){
            
            string vals = string.Join("|", values);

            uriBuilder.Query += uriBuilder.Query == "?" ? "" : "&";
            uriBuilder.Query += Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(vals);
        }
        
        ///-------------------------------------///
        ///             ACCESSORS               ///
        ///-------------------------------------///

        

        public override string ToString(){
            if (!uriBuilder.Uri.ToString().Contains("format=")){
                AppendParam("format", format.ToString().ToLower());
            }
            else{
                if (!uriBuilder.Uri.ToString().Contains("format=" + format.ToString().ToLower())){
                    string temp = uriBuilder.ToString().Replace("format=" + format.ToString().ToLower(), "");
                    uriBuilder = new UriBuilder(temp);
                    AppendParam("format", format.ToString().ToLower());
                }
            }
            return uriBuilder.ToString();
        }

        public Format Format{
            get{ return format; }
            set{ format = value; }
        }
    }

    public class InvalidUriException : Exception {
        public InvalidUriException(string message) : base(message){}
    }
}