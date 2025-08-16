# Manual de Usuario - Sistema de Gestión de Ventas y Cuenta Corriente

## Índice
1. [Introducción](#introducción)
2. [Pantalla Principal](#pantalla-principal)
3. [Gestión de Clientes](#gestión-de-clientes)
4. [Gestión de Artículos](#gestión-de-artículos)
5. [Registro de Ventas](#registro-de-ventas)
6. [Gestión de Cuotas](#gestión-de-cuotas)
7. [Trabajos de Aire Acondicionado](#trabajos-de-aire-acondicionado)
8. [Configuración del Sistema](#configuración-del-sistema)
9. [Reportes y PDFs](#reportes-y-pdfs)
10. [Ingresos y Egresos](#ingresos-y-egresos)
11. [Gestión de Stock](#gestión-de-stock)
12. [Consejos y Buenas Prácticas](#consejos-y-buenas-prácticas)

---

## Introducción

**Sistema de Gestión de Ventas y Cuenta Corriente** es una aplicación integral diseñada para administrar ventas, clientes, stock, cuotas y trabajos especializados de aire acondicionado. El sistema permite llevar un control detallado de todas las operaciones comerciales con funcionalidades avanzadas de reportes y seguimiento financiero.

### Características Principales:
- ✅ Gestión completa de clientes y artículos
- ✅ Ventas con sistema de cuotas flexibles
- ✅ Control automático de stock
- ✅ Gestión de trabajos de aire acondicionado
- ✅ Reportes en PDF personalizados
- ✅ Control financiero de ingresos y egresos
- ✅ Sistema de anticipos y métodos de pago

---

## Pantalla Principal

Al iniciar el sistema, se presenta el **menú principal** con acceso a todas las funcionalidades:

### Opciones del Menú:
- **Ventas**: Registro y gestión de ventas
- **Cuotas**: Administración de pagos y cuotas
- **Clientes**: ABM de clientes
- **Artículos**: Gestión de productos y stock
- **Aire Acondicionado**: Trabajos especializados
- **Ingresos y Egresos**: Control financiero
- **Configuración**: Marcas, conectores y medidas

### Barra de Configuración:
En la parte superior encontrará el menú **"Configuración"** que incluye:
- Marcas
- Conectores
- Medidas

---

## Gestión de Clientes

### Agregar Nuevo Cliente
1. Ir a **Clientes** desde el menú principal
2. Completar los campos obligatorios:
   - **Nombre** (obligatorio)
   - **Apellido** (obligatorio)
   - **DNI** (obligatorio, solo números)
   - **Dirección** (opcional)
   - **Teléfono** (opcional)
3. Hacer clic en **"Guardar"**

### Buscar Clientes
- **Por Apellido**: Busca coincidencias en el apellido
- **Por Nombre**: Busca coincidencias en el nombre
- **Por DNI**: Busca por número de documento

### Editar Cliente
1. Seleccionar el cliente en la grilla
2. Hacer clic en **"Editar"**
3. Modificar los datos necesarios
4. Hacer clic en **"Guardar"**

### Eliminar Cliente
1. Seleccionar el cliente en la grilla
2. Hacer clic en **"Eliminar"**
3. Confirmar la operación

> **⚠️ Importante**: No se puede eliminar un cliente que tenga ventas o cuotas asociadas.

---

## Gestión de Artículos

### Agregar Nuevo Artículo
1. Ir a **Artículos** desde el menú principal
2. Completar la información:
   - **Descripción** (obligatorio)
   - **Precio de Venta** (obligatorio)
   - **Stock Inicial**
   - **Marca** (seleccionar desde combo)
   - **Medida** (opcional)
   - **Tipo de Conector** (opcional)
3. Hacer clic en **"Guardar"**

### Control de Stock
- El sistema actualiza automáticamente el stock con cada venta
- Se puede consultar el stock disponible en tiempo real
- Alertas cuando el stock es insuficiente para una venta

### Búsqueda de Artículos
- Buscar por nombre o descripción
- Filtrar por marca
- Ver artículos activos/inactivos

---

## Registro de Ventas

### Crear Nueva Venta
1. Ir a **Ventas** desde el menú principal
2. **Seleccionar Cliente**:
   - Usar el combo desplegable, o
   - Hacer clic en **"Buscar Cliente"** para abrir el selector
   - Si el cliente no existe, se puede crear uno nuevo desde el selector
3. **Seleccionar Artículo**:
   - Usar el combo desplegable, o
   - Hacer clic en **"Buscar Artículo"** para abrir el selector
   - El precio se completa automáticamente
4. **Configurar la Venta**:
   - **Cantidad**: Unidades a vender
   - **Precio**: Se completa automáticamente (editable)
   - **Forma de Pago**: Mensual, Quincenal, Semanal
   - **Cuotas**: Número de cuotas
   - **Anticipo**: Monto del anticipo (opcional)
   - **Fecha de Venta**: Se completa automáticamente
5. Hacer clic en **"Registrar Venta"**

### Cálculos Automáticos
- **Subtotal**: Cantidad × Precio
- **Total Final**: Subtotal - Anticipo
- **Fecha de Cancelación**: Se calcula automáticamente según la forma de pago

### Editar Venta
1. Hacer doble clic en una venta de la grilla
2. Modificar los datos necesarios
3. Hacer clic en **"Editar Venta"**

### Eliminar Venta
1. Seleccionar la venta en la grilla
2. Hacer clic en **"Anular Venta"**
3. Confirmar la operación
4. El stock se restaura automáticamente

### Imprimir Comprobante de Venta
1. Seleccionar la venta en la grilla
2. Hacer clic en **"Imprimir Venta"**
3. Elegir ubicación para guardar el PDF

---

## Gestión de Cuotas

### Acceder a Cuotas
1. Ir a **Cuotas** desde el menú principal
2. Ver todas las cuotas pendientes y pagadas

### Registrar Pago de Cuota
1. Seleccionar la cuota en la grilla
2. Hacer clic en **"Pagar Cuota"**
3. Seleccionar **Método de Pago**:
   - Efectivo
   - Transferencia
   - Tarjeta de Débito
   - Tarjeta de Crédito
4. Confirmar el pago

### Estados de Cuotas
- **🔴 Vencidas**: Cuotas con fecha de vencimiento pasada
- **🟡 Próximas a vencer**: Cuotas que vencen pronto
- **🟢 Pagadas**: Cuotas ya abonadas

### Búsqueda de Cuotas
- **Por Cliente**: Ver cuotas de un cliente específico
- **Por Estado**: Filtrar por pendientes/pagadas
- **Por Fecha**: Filtrar por rango de fechas

### Imprimir Recibo de Pago
1. Seleccionar la cuota pagada
2. Hacer clic en **"Imprimir Recibo"**
3. Se genera un PDF con el recibo de pago

---

## Trabajos de Aire Acondicionado

### Registrar Nuevo Trabajo
1. Ir a **Aire Acondicionado** desde el menú principal
2. Completar la información:
   - **Cliente** (buscar o seleccionar)
   - **Descripción del Trabajo**
   - **Monto**
   - **Fecha del Trabajo**
3. Hacer clic en **"Guardar"**

### Características Especiales
- Los trabajos de A/A se registran como ingresos directos
- No generan cuotas (son pagos únicos)
- Aparecen destacados en **amarillo** en el reporte de ingresos y egresos
- Se incluyen en los reportes financieros

---

## Configuración del Sistema

### Acceder a Configuración
Hacer clic en **"Configuración"** en la barra superior del menú principal.

### Gestión de Marcas
1. Seleccionar **"Marcas"**
2. Agregar, editar o eliminar marcas de productos
3. Las marcas se usan al registrar artículos

### Gestión de Conectores
1. Seleccionar **"Conectores"**
2. Gestionar tipos de conectores para productos especializados

### Gestión de Medidas
1. Seleccionar **"Medidas"**
2. Administrar las medidas disponibles para productos

---

## Reportes y PDFs

### Historial de Ventas por Cliente
1. Ir a **Cuotas**
2. Seleccionar un cliente
3. Hacer clic en **"Historial PDF"**
4. Se genera un reporte completo con:
   - Datos del cliente
   - Historial de ventas
   - Estado de cuotas con fechas de vencimiento
   - **Indicadores visuales**:
     - 🔴 Cuotas vencidas (fondo rojo)
     - 🟡 Próximas a vencer (fondo amarillo)
     - 🟢 Pagadas (fondo verde)
   - Estadísticas resumidas

### Comprobantes de Venta
- Se generan automáticamente al imprimir una venta
- Incluyen todos los detalles del producto y cliente
- Formato profesional con subtotal, anticipo y total

### Recibos de Pago
- Se generan al imprimir el recibo de una cuota pagada
- Incluyen método de pago utilizado
- Detalles del cliente y la cuota

---

## Ingresos y Egresos

### Acceder al Reporte Financiero
1. Ir a **"Ingresos y Egresos"** desde el menú principal
2. Configurar el rango de fechas
3. Hacer clic en **"Filtrar"**

### Tipos de Movimientos Mostrados

#### Ingresos (Verde) 💚
- **Ventas**: Ingresos por ventas registradas
- **Cuotas Pagadas**: Pagos de cuotas recibidas
- **Trabajos A/A**: Trabajos de aire acondicionado (destacados en **amarillo** 💛)

#### Egresos (Rojo) ❤️
- **Compras**: Compras de mercadería
- **Gastos**: Otros gastos del negocio

### Totales y Estadísticas
El sistema muestra:
- **Total Ventas**: Suma de todas las ventas del período
- **Total Cuotas Pagadas**: Con desglose por método de pago
  - Ejemplo: `Total Cuotas: $15,000 (Efec: $5,000, Trans: $7,000, Tarj: $3,000)`
- **Total Trabajos A/A**: Ingresos por trabajos de aire acondicionado
- **Total Egresos**: Suma de todos los gastos

### Filtros Disponibles
- **Fecha Desde/Hasta**: Rango de fechas personalizable
- **Actualización en tiempo real**: Los datos se actualizan al cambiar las fechas

---

## Gestión de Stock

### Control Automático
- El stock se actualiza automáticamente con cada venta
- Se descuenta la cantidad vendida del stock disponible
- Al eliminar una venta, el stock se restaura

### Validaciones
- **Stock insuficiente**: El sistema alerta si no hay stock suficiente
- **Control en tiempo real**: Verificación antes de cada venta

### Consulta de Stock
- Ver stock disponible en la gestión de artículos
- Stock actualizado en tiempo real

---

## Consejos y Buenas Prácticas

### 🎯 Gestión de Clientes
- Mantener siempre actualizada la información de contacto
- Usar el DNI como identificador único
- Verificar duplicados antes de crear nuevos clientes

### 💰 Gestión de Ventas
- Verificar stock disponible antes de prometer productos
- Configurar correctamente la forma de pago para cálculos precisos
- Utilizar anticipos para mejorar el flujo de caja

### 📅 Gestión de Cuotas
- Revisar regularmente las cuotas vencidas
- Registrar los pagos inmediatamente cuando se reciben
- Utilizar diferentes métodos de pago para mejor control

### 📊 Reportes Financieros
- Revisar semanalmente el reporte de ingresos y egresos
- Utilizar los filtros de fecha para análisis específicos
- Generar PDFs para conservar registros históricos

### 🔧 Mantenimiento
- Hacer backup regular de la base de datos
- Mantener actualizada la información de marcas y medidas
- Revisar periódicamente el stock para detectar discrepancias

### 🎨 Códigos de Color
- **Verde**: Ingresos, cuotas pagadas
- **Amarillo**: Trabajos de A/A, cuotas próximas a vencer
- **Rojo**: Egresos, cuotas vencidas
- **Azul**: Encabezados de tablas

---

## Versión Demo vs Versión Completa

### Limitaciones de la Versión Demo
- Máximo 5 ventas registradas
- Funcionalidades completas para evaluación
- Mensaje de aviso al alcanzar el límite

### Versión Completa
- Sin limitaciones en el número de registros
- Soporte técnico incluido
- Actualizaciones futuras

---

## Soporte Técnico

Para soporte técnico o consultas adicionales:
- Contactar al desarrollador del sistema
- Mantener actualizada la versión del software
- Reportar cualquier problema o sugerencia de mejora

---

**© 2025 Sistema de Gestión de Ventas y Cuenta Corriente**
*Manual de Usuario - Versión 1.0*
