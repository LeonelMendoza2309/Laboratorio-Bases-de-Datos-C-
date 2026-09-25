<div align="center">

# UNIVERSIDAD TECNOLOGICA DE PANAMÁ  
Facultad de Ingeniería en Sistemas Y Computación  
Lic. en Ing. de Sistemas Computacionales

</div>

---

# HPAIII - Laboratorio: Métodos Estáticos y Nuevos Controles

**Autor:** Leonel Mendoza (1IL133)

---

## Estructura del Repositorio (carpetas con actividades)

A continuación se muestran las carpetas con los proyectos desarrollados en este laboratorio:

```
Laboratorio-Validaciones---M-todos-Est-ticos-y-Nuevos-Controles/
├── HPAIII-Laboratorio#3                          # Proyecto C#: DataGridView con métodos estáticos
├── Juego_De_Craps_lab3                           # Proyecto C#: Juego de Craps con enumeradores
├── MDI-Forms                                      # Proyecto C#: Aplicación MDI (Multiple Document Interface)
└── README.md                                      # Documentación del repositorio
```

---

## Tabla de Contenidos
- [Objetivo del Laboratorio](#objetivo-del-laboratorio)
- [Descripción de Proyectos](#descripción-de-proyectos)
- [Tecnología principal](#tecnología-principal)
- [Requisitos](#requisitos)
- [Instalación y ejecución](#instalación-y-ejecución)
- [Problemas y soluciones](#problemas-y-soluciones)
- [Resultados](#resultados)
- [Conclusiones](#conclusiones)

---

## Objetivo del Laboratorio

El objetivo principal de este laboratorio es:
- Comprender y aplicar métodos estáticos en C#.
- Trabajar con controles avanzados de Windows Forms (DataGridView, MDI).
- Implementar validaciones mediante expresiones regulares.
- Utilizar enumeradores para estados y valores constantes.
- Desarrollar interfaces gráficas con MDI (Multiple Document Interface).
- Aplicar conceptos de encapsulamiento e interfaces en aplicaciones de escritorio.

---

## Descripción de Proyectos

### 1. HPAIII-Laboratorio#3 (DataGridView y Métodos Estáticos)
**Descripción:** Aplicación que demuestra el uso de DataGridView con una clase modelo `Persona` y validaciones mediante métodos estáticos.

**Componentes principales:**
- `Persona.cs`: Clase modelo con propiedades (ID, Nombre, Apellidos, Correo, FechaNacimiento, Salario)
- `Utilidades.cs`: Clase con métodos estáticos para validación de correo y cadenas vacías
- `Form1.cs`: Formulario Windows con DataGridView para gestionar datos de personas

**Tecnologías:** Windows Forms, DataGridView, Regex, Métodos estáticos

### 2. Juego_De_Craps_lab3 (Enumeradores y Lógica de Juego)
**Descripción:** Implementación del juego de Craps (juego de dados) utilizando enumeradores para estados del juego.

**Componentes principales:**
- `Craps.cs`: Clase que implementa la lógica del juego con:
  - Enumerador `Estado` (CONTINUAR, GANAR, PERDER)
  - Enumerador `NombDados` (valores especiales del juego)
  - Método `Jugar()`: Control del flujo del juego
  - Método `TirarDados()`: Simula el lanzamiento de dos dados

**Tecnologías:** Enumeradores, Switch case, Números aleatorios

### 3. MDI-Forms (Multiple Document Interface)
**Descripción:** Aplicación con interfaz MDI que permite abrir múltiples ventanas secundarias.

**Componentes principales:**
- `Form1.cs`: Formulario principal (MDI Parent) con barra de herramientas
- `Form2.cs`: Formulario secundario
- `frmVentanaTexto.cs`: Ventana de texto reutilizable
- Menú y botones de herramientas para crear nuevas ventanas

**Tecnologías:** Windows Forms, MDI, Controles de usuario, ToolStrip

---

## Tecnología principal

- Lenguaje: C# (curso centrado en este lenguaje)
- Plataforma / Runtime: .NET Framework 4.7.2
- Entorno de desarrollo sugerido: Visual Studio Community 2019 o superior
- Compilador: Roslyn (csc)
- Control de versiones y alojamiento: Git / GitHub
- Controles: Windows Forms, DataGridView, ToolStrip

---

## Requisitos

Requisitos mínimos para compilar y ejecutar los proyectos:
- .NET Framework 4.7.2 o superior instalado
- Visual Studio 2019 o 2022 (recomendado) o Visual Studio Code + extensión C#
- Git para clonar el repositorio
- Sistema operativo: Windows 10 o superior (requerido para Windows Forms)

---

## Instalación y ejecución

Pasos en una máquina con .NET Framework instalado.

### Clonar el repositorio:

```bash
git clone https://github.com/LeonelMendoza2309/Laboratorio-Validaciones---M-todos-Est-ticos-y-Nuevos-Controles.git
cd Laboratorio-Validaciones---M-todos-Est-ticos-y-Nuevos-Controles
```

### Ejecutar con Visual Studio (GUI):
1. Abrir Visual Studio 2019 o superior.
2. Abrir la carpeta del repositorio o los archivos `.sln` desde Archivo → Abrir.
3. En el Explorador de soluciones, seleccionar el proyecto que desea ejecutar.
4. Marcar como "Startup Project" (clic derecho → Set as Startup Project).
5. Ejecutar con F5 (depuración) o Ctrl+F5 (sin depuración).

### Ejecutar cada proyecto por separado:

#### HPAIII-Laboratorio#3:
```bash
cd HPAIII-Laboratorio#3
```
- Abrir `HPAIII-Laboratorio#3.sln` con Visual Studio
- Ejecutar con F5

#### Juego_De_Craps_lab3:
```bash
cd Juego_De_Craps_lab3
```
- Abrir `Juego_De_Craps_lab3.sln` con Visual Studio
- Ejecutar con F5
- Introducir datos cuando se solicite en la consola

#### MDI-Forms:
```bash
cd MDI-Forms
```
- Abrir `MDI-Forms.sln` con Visual Studio
- Ejecutar con F5
- Utilizar el menú o botones de herramientas para abrir nuevas ventanas

### Notas adicionales:
- Los proyectos de juego leen entrada desde consola (`Console.ReadLine`). Introduce los datos cuando se te solicite.
- Para limpiar artefactos de compilación: clic derecho en solución → Limpiar solución.
- MDI-Forms es una aplicación gráfica que requiere Windows.

---

## Problemas y soluciones

Lista de problemas detectados durante el desarrollo y cómo se resolvieron:

| # | Problema | Descripción | Acción tomada |
|---:|---------|-------------|---------------|
| 1 | Validación de correo inválida | Expresión regular incompleta no validaba correctamente emails | Mejorar el patrón Regex: `@"^[^@\s]+@[^@\s]+\.[^@\s]+$"` |
| 2 | Métodos no estáticos | Métodos de utilidad requería instanciación | Cambiar a métodos `static` en clase `Utilidades` |
| 3 | Lógica del juego Craps | Flujo incompleto del juego no reconocía todos los escenarios | Implementar enumeradores y switch case para manejar estados |
| 4 | Espacios en blanco en validaciones | Validación no consideraba espacios en blanco | Usar `string.IsNullOrEmpty()` y `string.IsNullOrWhiteSpace()` |

---

## Resultados

### Funcionalidades implementadas destacadas:


---

## Conclusiones

- Se comprendieron correctamente los métodos estáticos y su aplicación en validaciones.
- Las expresiones regulares son herramientas poderosas para validación de datos.
- Las interfaces MDI permiten crear aplicaciones complejas con múltiples ventanas.
- Los controles de Windows Forms como DataGridView son útiles para mostrar datos estructurados.

---

**Última actualización:** 13/09/2026   
**Estado:** Completado y entregado
