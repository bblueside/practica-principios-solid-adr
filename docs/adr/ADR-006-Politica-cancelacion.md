# ADR-006: Extraer la política de cancelación y obtener la hora desde TimeProvider

* Estado: Aceptado
* Fecha: 2026-10-05
* Autores: Juliana Velandia

## Contexto

En el Legacy, `CancelarCita` aplicaba la regla de negocio directamente: si se cancela con menos de 24 h, la penalización es 50, y Cirugía suma 40. Además, recibía la hora de cancelación como un `DateTime` suelto. Esto tenía dos problemas:
* la regla de cancelación quedaba mezclada con persistencia y notificación (viola SRP);
* el Gestor dependía de un valor de reloj que el llamador construía con `DateTime.Now`.

## Decisión

Extraeremos la regla a `PoliticaCancelacion`, detrás de la interfaz `IPoliticaCancelacion`. Su método `CalcularPenalidad(Cita, IEspecialidad)`:
* toma el monto de `IEspecialidad.RecargoCancelacion` ([ADR-003])
**Rationale:** la política de cancelación cambia por decisiones de negocio.

## Consecuencias

**Positivas**
* La regla de las 24 h queda en un único lugar y se puede reemplazar por otra política sin tocar el Gestor (SRP, OCP).
* El reloj se puede controlar, lo que da un comportamiento determinista.

**Negativas**
* El llamador debe calcular la penalización antes de llamar a `CancelarCita`, lo que agrega un paso al flujo (ver [ADR-008](ADR-008-Orquestacion-en-Program.md)).

## Cumplimiento (Compliance)

* Ninguna regla de penalización puede vivir fuera de `IPoliticaCancelacion`.
