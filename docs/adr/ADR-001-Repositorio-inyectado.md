# ADR-001: Inyectar la persistencia mediante la interfaz IRepositorio

* Estado: Aceptado
* Fecha: 2026-10-03
* Autores: Juliana Velandia

## Contexto

En `DentaCare.Legacy`, `GestorCitasOdontologicas` creaba su propio acceso a datos (`new SqlServerEjecutor()`) e importaba `System.Data.SqlClient`. Si se quiere cambiar el motor de base de datos o el mecanismo de almacenamiento se obliga a modificar el Gestor, lo que viola DIP. Además, el Gestor tenía 5 dependencias concretas (Ce = 5).

## Decisión

Definiremos la interfaz `IRepositorio`, con los métodos `GuardarCita` y `ActualizarCita`, y `SqlServerEjecutor` será su implementación. El Gestor recibirá el repositorio por constructor y solo conocerá la interfaz. La implementación concreta se elige en un único punto: `Program`.

**Rationale:** las reglas de negocio no deben depender de los detalles de infraestructura. Con la inversión de dependencias, la política (el Gestor) y el detalle (SQL Server) apuntan ambos a una abstracción estable.

## Consecuencias

**Positivas**
* El Gestor queda desacoplado de SQL Server. Se puede cambiar la persistencia sin tocarlo, lo que mejora la mantenibilidad y la portabilidad.

**Negativas**
* Hay un tipo más que mantener.
* Alguien tiene que componer las dependencias (clase `Program`).

## Cumplimiento (Compliance)

* **Revisión de código en el PR:** ninguna clase fuera de `Program` puede instanciar `SqlServerEjecutor`. El Gestor no debe tener `using System.Data.SqlClient`.
* **Métricas:** en cada iteración se vuelve a calcular el Ce de `GestorCitasOdontologicas`, que no debe superar 3 dependencias, todas abstracciones o entidades.
