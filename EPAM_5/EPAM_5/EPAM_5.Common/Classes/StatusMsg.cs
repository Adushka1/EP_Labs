namespace EPAM_5.Common
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.ugiu.com/sh/CommonStatusService")]
    public enum StatusMsg
    {
        
        SUCCESS,
        
        PARTIAL_SUCCESS,
        
        SYSTEM_TIMEOUT,
        
        SYSTEM_NOT_AVAILABLE,
        
        GENERAL_ERROR,
        
        DATA_VALIDATION_ERROR,
        
        AUTHENTICATION_ERROR,
        
        DATABASE_ERROR,
        
        NO_RECORDS_FOUND,
        
        FUTURE_USE,
        
        [System.Xml.Serialization.XmlEnumAttribute("FUTURE_USE")]
        FUTURE_USE1,
        
        BACKEND_ERROR,
    }
}
