using Newtonsoft.Json;
using System;

namespace JsonNetParse.StrictConverters
{
    /// <summary>
    /// Json Converter for numberic types which checks if the incoming Json
    /// element is of type Integer or Float and throws exception if it isn't.
    /// </summary>
    class NumberConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            switch (Type.GetTypeCode(objectType))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool isInt = reader.TokenType == JsonToken.Integer;
            bool isFloat = reader.TokenType == JsonToken.Float;

            if (!isInt && !isFloat)
            {
                throw new JsonReaderException(
                    $"TokenType must be Integer of Float, {reader.TokenType} found.");
            }

            // XXX: Could do more to check if conversions are ok.

            return Convert.ChangeType(reader.Value, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
