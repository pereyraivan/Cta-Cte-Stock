# Control-C Stock - Sistema de Ventas

## Estructura del Proyecto

Este proyecto está organizado en una arquitectura de 4 capas:

### 1. Capa de Entidades (CEntidades)
- Contiene las clases modelo generadas por Entity Framework
- Representa las entidades de la base de datos
- Ejemplo: `Cliente.cs`, `Venta.cs`, `Cuota.cs`

### 2. Capa de Datos (CDatos)
- Contiene los repositorios que se comunican con la base de datos
- Utiliza Entity Framework para operaciones CRUD
- Ejemplo: `RepositorioCliente.cs`, `RepositorioVenta.cs`
- Patrón de nombramiento: `Repositorio{EntidadNombre}.cs`

### 3. Capa de Lógica (CLogica)
- Contiene los gestores que manejan la lógica de negocio
- Actúa como intermediario entre presentación y datos
- Ejemplo: `GestorCliente.cs`, `GestorVenta.cs`
- Patrón de nombramiento: `Gestor{EntidadNombre}.cs`

### 4. Capa de Presentación (VentaCredimax)
- Contiene los formularios de Windows Forms
- Se comunica con la capa de lógica a través de los gestores

## Flujo de Datos

### Inserción de Datos
1. El usuario ingresa datos en un formulario
2. El formulario crea una instancia del gestor correspondiente
3. El gestor valida y procesa los datos
4. El gestor llama al repositorio correspondiente
5. El repositorio guarda los datos usando Entity Framework

### Modificación de Datos
1. El usuario modifica datos en un formulario
2. El formulario llama al gestor correspondiente
3. El gestor valida los cambios
4. El gestor llama al repositorio
5. El repositorio actualiza los datos en la base de datos

### Consulta de Datos
1. El formulario solicita datos al gestor
2. El gestor llama al repositorio correspondiente
3. El repositorio consulta la base de datos usando Entity Framework
4. Los datos fluyen de vuelta a través del gestor hasta el formulario

## Tecnologías Utilizadas
- .NET Framework
- Entity Framework
- Windows Forms
- SQL Server (base de datos)
