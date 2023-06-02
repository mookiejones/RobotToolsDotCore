using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RobotTools.Core.Utilities;

/// <summary>
///   Extensions for supporting xml serialization by <see cref = "XmlSerializer" />
/// </summary>
public static class XmlSerializerExtensions
{
    #region Private fields
    private static readonly Dictionary<RuntimeTypeHandle, XmlSerializer> _serializers = new Dictionary<RuntimeTypeHandle, XmlSerializer>();
    #endregion
    #region Public methods
    /// <summary>
    ///   Serialize object to xml string by <see cref = "XmlSerializer" />
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value"></param>
    /// <returns></returns>
    public static string ToXml<T>(this T value)
        where T : new()
    {
        var _serializer = GetValue(typeof(T));
        using var _stream = new MemoryStream();
        using var _writer = new XmlTextWriter(_stream, new UTF8Encoding());
        _serializer.Serialize(_writer, value);
        return Encoding.UTF8.GetString(_stream.ToArray());
    }
    public static void Serialize<T>(this T value,string filePath)
    {
        var serializer = GetValue(typeof(T));
        using var writer = new StreamWriter(filePath);
        serializer.Serialize(writer, value);
    }

    /// <summary>
    ///   Serialize object to stream by <see cref = "XmlSerializer" />
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value"></param>
    /// <param name = "stream"></param>
    public static void ToXml<T>(this T value, Stream stream)
        where T : new()
    {
        var _serializer = GetValue(typeof(T));
        _serializer.Serialize(stream, value);
    }

    /// <summary>
    ///   Deserialize object from string
    /// </summary>
    /// <typeparam name = "T">Type of deserialized object</typeparam>
    /// <param name = "srcString">Xml source</param>
    /// <returns></returns>
    public static T FromXml<T>(this string srcString)
        where T : new()
    {
        var _serializer = GetValue(typeof(T));
        using var _stringReader = new StringReader(srcString);
        using XmlReader _reader = new XmlTextReader(_stringReader);
        return (T)_serializer.Deserialize(_reader);
    }
    /// <summary>
    ///   Deserialize object from stream
    /// </summary>
    /// <typeparam name = "T">Type of deserialized object</typeparam>
    /// <param name = "source">Xml source</param>
    /// <returns></returns>
    public static T FromXml<T>(this Stream source)
        where T : new()
    {
        var _serializer = GetValue(typeof(T));
        return (T)_serializer.Deserialize(source);
    }
    #endregion
    #region Private methods
    private static XmlSerializer GetValue(Type type)
    {
        XmlSerializer _serializer;
        if (!_serializers.TryGetValue(type.TypeHandle, out _serializer))
        {
            lock (_serializers)
            {
                if (!_serializers.TryGetValue(type.TypeHandle, out _serializer))
                {
                    _serializer = new XmlSerializer(type);
                    _serializers.Add(type.TypeHandle, _serializer);
                }
            }
        }
        return _serializer;
    }
    #endregion
}
