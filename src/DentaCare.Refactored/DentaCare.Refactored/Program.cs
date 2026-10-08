using DentaCare.Refactored;

Console.WriteLine("=================================================");
Console.WriteLine(" DENTACARE SYSTEM - MÓDULO LEGADO DE CITAS"); 
Console.WriteLine("=================================================\\n"); 

try 
{
    GestorCitasOdontologicas gestor = new GestorCitasOdontologicas(new SqlServerEjecutor(), new SMS()); 

    Paciente paciente1 = new Paciente 
    { 
        Id = "PAC-101", 
        NombreCompleto = "Ana María Gómez", 
        Correo = "ana.gomez@email.com", 
        Celular = "3001234567", TipoConvenio = 2, // 2 = EPS (Convenio del 70% de cobertura)
        EsPrimeraVez = true 
    }; 
    
    Odontologo odontologo1 = new Odontologo 
    { 
        Id = "ODO-202", 
        Nombre = "Dr. Roberto Martínez", 
        EspecialidadId = 3, // 3 = Cirugía
        EstaDisponible = true
    }; 
    
    Console.WriteLine("---> [FLUJO 1]: AGENDAMIENTO DE CITA ODONTOLÓGICA"); 

    if (!odontologo1.EstaDisponible)
        {
            throw new InvalidOperationException("El odontólogo no tiene disponibilidad en el horario seleccionado.");
        }

        ICalculadoraCopago calculadoraCopago = new CalculadoraCopago();
        IPoliticaCancelacion politicaCancelacion = new PoliticaCancelacion(TimeProvider.System);
        Metricas metricas = new Metricas();
        IEspecialidad especialidad = new Cirugia(); // EspecialidadId = 3 (Cirugía)
        IConvenio convenio = new EPS(); // TipoConvenio = 2 (EPS)
        decimal copagoFinal = calculadoraCopago.CalcularCopago(especialidad, convenio, paciente1.EsPrimeraVez, true);
        metricas.RegistrarRecaudo(copagoFinal);

        Cita nuevaCita = new Cita(
            Guid.NewGuid().ToString().Substring(0, 8),
            paciente1,
            odontologo1,
            DateTime.Now.AddHours(12),
            copagoFinal,
            "PROGRAMADA",
            0m
        );

    Cita citaAgendada = gestor.AgendarCita(nuevaCita); 

    Console.WriteLine($"\n Cita generada exitosamente con ID: {citaAgendada.Id}"); 
    Console.WriteLine($" Copago Final Calculado: ${citaAgendada.CopagoCalculado:N2}"); 
    Console.WriteLine($" Estado de la Cita: {citaAgendada.Estado}\\n"); 

    Console.WriteLine("-------------------------------------------------"); 
    Console.WriteLine("---> [FLUJO 2]: CANCELACIÓN DE CITA (MENOS DE 24 HORAS)");


    citaAgendada.setEstado("CANCELADA");
    decimal penalizacion = politicaCancelacion.CalcularPenalidad(citaAgendada, especialidad);
    gestor.CancelarCita(citaAgendada, penalizacion);
    
    Console.WriteLine($"\n Cita ID #{citaAgendada.Id} ha sido actualizada."); 
    Console.WriteLine($" Nuevo Estado: {citaAgendada.Estado}");     
    Console.WriteLine($" Penalización Aplicada: ${penalizacion:N2}\\n"); 
    metricas.RegistrarCancelacion();

    Console.WriteLine("-------------------------------------------------"); 
    Console.WriteLine("---> [FLUJO 3]: REPORTE DE TOTALES ACUMULADOS EN MEMORIA");
    Console.WriteLine($" Total Recaudado en Copagos: ${metricas.recaudos_totales:N2}"); 
    Console.WriteLine($" Total Citas Canceladas: {metricas._totalCitasCanceladas}"); 
} 
catch (Exception ex) 
{ 
    Console.WriteLine($"\n ERROR CRÍTICO EN EL SISTEMA: {ex.Message}"); 
} 

Console.WriteLine("\n================================================="); 
Console.WriteLine("Presione cualquier tecla para salir..."); 
Console.ReadKey();