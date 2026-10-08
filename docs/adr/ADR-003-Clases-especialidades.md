# ADR-003: Modelar las especialidades en clases aparte

* Estado: Aceptado
* Fecha: 2026-10-03
* Autores: Juliana Velandia

## Contexto

En el Legacy, las especialidades se identificaban con un `int` (1 = Ortodoncia, 2 = Endodoncia, 3 = Cirugía, 4 = Odontopediatría). Sus reglas estaban repartidas en dos cadenas de `if/else`:
* el factor de copago, en `AgendarCita`;
* el recargo extra por cancelar una cirugía (+40), en `CancelarCita`.

Agregar o modificar una especialidad obligaba a editar el Gestor en varios lugares (viola OCP).

## Decisión

Se crea la interfaz `IEspecialidad` que contiene el método `CalcularCostoEspecialidad()` (el factor de copago) y `RecargoCancelacion` (la penalización por cancelar con menos de 24 h). Cada especialidad será una clase: `Ortodoncia`,  `Endodoncia`, `Cirugia` y `Ortopediatria`.

**Rationale:** todo el comportamiento de una especialidad queda en un único lugar. Cirugía es la única clase que implementa el valor Recargo = 90 por su condición especial.

## Consecuencias

**Positivas**
* Una especialidad nueva se agrega con una clase nueva, sin tocar el cálculo ni la cancelación (OCP).
* Desaparecen los `if/else` por tipo, lo que mejora la legibilidad y reduce la complejidad.

**Negativas**
* Hay más clases pequeñas en el proyecto.
* Los valores siguen escritos en el código. Cambiar una tarifa requiere recompilar.

## Cumplimiento (Compliance)

* **Métricas:** la complejidad de `CalculadoraCopago` y de `PoliticaCancelacion` no crece al agregar especialidades.
