# ADR-008: Componer dependencias y orquestar el flujo en Program

* Estado: Aceptado (transitorio)
* Fecha: 2026-10-06
* Autores: Juliana Velandia

## Contexto

Las decisiones anteriores ([ADR-001]a [ADR-007]) sacaron del Gestor el cálculo del copago, la penalización, las estadísticas y la creación de la infraestructura. Faltaba definir quién instancia las implementaciones concretas y quién encadena los pasos del flujo:
1. validar la disponibilidad del odontólogo;
2. calcular el copago;
3. crear la `Cita`;
4. calcular la penalización;
5. registrar las métricas.

## Decisión

`Program` es una clase orquestadora. Ahí se crean `SqlServerEjecutor`, `SMS`, `CalculadoraCopago`, `PoliticaCancelacion` y `Metricas`, y también se ejecutan los pasos del flujo. Para soportarlo:
* `Cita` tiene un constructor con todos sus datos y el método `setEstado`;
* el Gestor queda con dos firmas simples: `AgendarCita(Cita)` y `CancelarCita(Cita, decimal penalizacion)`. Su única tarea es persistir y notificar.

**Rationale:** concentrar la creación de objetos en un único punto hace efectiva la inversión de dependencias. Además, mantiene el Gestor mínimo (Ce = 3, LCOM96b = 0.00) mientras se define un caso de uso dedicado.

## Consecuencias

**Positivas**
* Las implementaciones concretas solo aparecen en `Program`, así que cambiar una implementación implica una sola edición.
* El Gestor queda pequeño, cohesivo y sin reglas de negocio.

**Negativas**
* `Program` acumula reglas de negocio (validar disponibilidad, armar la cita, cambiar el estado).

## Cumplimiento (Compliance)

* La palabra `new` sobre clases de infraestructura (`SqlServerEjecutor`, `Email`, `SMS`) solo se encuentra en `Program`.
