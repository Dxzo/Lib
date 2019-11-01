### Dxzo.Data
Es necesario tener una version .net framework >= 4.5.2

### Uso
```
  DataAccess data = new MySqlDataAccess();
  var datatable = data.ExecuteQuery("select * from table");
```

### Setup
Configuracion para log4net
```
<!-- Configuracion para log4net -->
<configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
</configSections>
<log4net>
  <appender name="FileAppender" type="log4net.Appender.FileAppender">
    <file value="dxzo-data.log"/>
    <appendToFile value="true"/>
      <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date [%thread] %-5level %logger - %message%newline"/>
    </layout>
  </appender>
  <root>
      <level value="ALL"/>
      <appender-ref ref="FileAppender"/>
  </root> 
</log4net>
<!-- Configuracion para log4net -->
```
Propiedades para log4net
```
<appSettings>
  <add key="log4net.Config" value="App.exe.config"/>  <!--Determina cual es el archivo de configuracion para los logs--> 
  <add key="log4net.Config.Watch" value="True"/>  <!--Determina si el archivo debe ser monitoreado para detectar cambios--> 
</appSettings>
  ```
Propiedades de conexion por defecto
```
<connectionStrings>
  <add name="MySql" connectionString="Server=;Database=;Uid=;Pwd=;"/>
  <add name="SqlServer" connectionString="Server=;Database=;User Id=;Password=;"/>
</connectionStrings>
```

### Nuget
* [Dxzo.Data](https://www.nuget.org/packages/Dxzo.Data/)
