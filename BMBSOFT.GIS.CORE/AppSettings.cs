namespace BMBSOFT.GIS.CORE
{
    public class AppSettings
    {
        public int AccessTokenExpireTimeSpan { get; set; }
        public int TokenExpirationInMinute { get; set; }
        public int MinimumRequiredLength { get; set; }
        public string BaseUrl { get; set; }
        public int DefaultPageSize { get; set; }
        public string UrlLogin { get; set; }
        public string GeoServer { get; set; }
        public string OriginGeoServer { get; set; }
        public string KeyPoligon { get; set; }
        public string DefaultStyle { get; set; }
        public string WorkSpace { get; set; }
        public string DataStore { get; set; }
        public string Schema { get; set; }
        public string SchemaBoudaries { get; set; }
        public string PropertyName { get; set; }
        public string LandType { get; set; }
        public string PgVersion { get; set; }
        public string SrId { get; set; }
        public string DefaultProjection { set; get; }
        public int QueryTimeOut { get; set; }
        public string GoogleAPIKey { get; set; }
    }
}
