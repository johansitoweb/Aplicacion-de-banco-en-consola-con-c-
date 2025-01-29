using System;
using System.Collections.Generic;

namespace BancoConsola
{
    class Program
    {
        static List<CuentaBancaria> cuentas = new List<CuentaBancaria>();
        static CuentaBancaria cuentaActual;

        static void Main(string[] args)
        {
            // Crear algunas cuentas de ejemplo
            cuentas.Add(new CuentaBancaria("Juan Pérez", "1234", 1000));
            cuentas.Add(new CuentaBancaria("Maria Lopez", "5678", 2000));

            if (AutenticarUsuario())
            {
                int opcion;

                do
                {
                    Console.WriteLine("Bienvenido al Banco");
                    Console.WriteLine("1. Consultar Saldo");
                    Console.WriteLine("2. Depositar Dinero");
                    Console.WriteLine("3. Retirar Dinero");
                    Console.WriteLine("4. Transferir Dinero");
                    Console.WriteLine("5. Salir");
                    Console.Write("Seleccione una opción: ");
                    opcion = int.Parse(Console.ReadLine());

                    switch (opcion)
                    {
                        case 1:
                            Console.WriteLine($"Saldo actual: {cuentaActual.ConsultarSaldo()}");
                            break;
                        case 2:
                            Console.Write("Ingrese la cantidad a depositar: ");
                            decimal cantidadDepositar = decimal.Parse(Console.ReadLine());
                            cuentaActual.Depositar(cantidadDepositar);
                            Console.WriteLine("Depósito realizado con éxito.");
                            break;
                        case 3:
                            Console.Write("Ingrese la cantidad a retirar: ");
                            decimal cantidadRetirar = decimal.Parse(Console.ReadLine());
                            if (cuentaActual.Retirar(cantidadRetirar))
                            {
                                Console.WriteLine("Retiro realizado con éxito.");
                            }
                            else
                            {
                                Console.WriteLine("Fondos insuficientes.");
                            }
                            break;
                        case 4:
                            Console.Write("Ingrese el número de cuenta destino: ");
                            string cuentaDestino = Console.ReadLine();
                            Console.Write("Ingrese la cantidad a transferir: ");
                            decimal cantidadTransferir = decimal.Parse(Console.ReadLine());
                            if (cuentaActual.Transferir(cuentaDestino, cantidadTransferir, cuentas))
                            {
                                Console.WriteLine("Transferencia realizada con éxito.");
                            }
                            else
                            {
                                Console.WriteLine("Fondos insuficientes o cuenta destino no encontrada.");
                            }
                            break;
                        case 5:
                            Console.WriteLine("Gracias por usar el Banco. ¡Hasta luego!");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Intente de nuevo.");
                            break;
                    }

                    Console.WriteLine();
                } while (opcion != 5);
            }
            else
            {
                Console.WriteLine("Autenticación fallida. Programa terminado.");
            }
        }

        static bool AutenticarUsuario()
        {
            Console.Write("Ingrese su nombre de usuario: ");
            string usuario = Console.ReadLine();
            Console.Write("Ingrese su PIN: ");
            string pin = Console.ReadLine();

            foreach (var cuenta in cuentas)
            {
                if (cuenta.Titular == usuario && cuenta.PIN == pin)
                {
                    cuentaActual = cuenta;
                    return true;
                }
            }

            return false;
        }
    }

    class CuentaBancaria
    {
        public string Titular { get; set; }
        public string PIN { get; set; }
        public decimal Saldo { get; private set; }

        public CuentaBancaria(string titular, string pin, decimal saldoInicial)
        {
            Titular = titular;
            PIN = pin;
            Saldo = saldoInicial;
        }

        public decimal ConsultarSaldo()
        {
            return Saldo;
        }

        public void Depositar(decimal cantidad)
        {
            Saldo += cantidad;
        }

        public bool Retirar(decimal cantidad)
        {
            if (cantidad <= Saldo)
            {
                Saldo -= cantidad;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Transferir(string cuentaDestino, decimal cantidad, List<CuentaBancaria> cuentas)
        {
            if (cantidad <= Saldo)
            {
                foreach (var cuenta in cuentas)
                {
                    if (cuenta.Titular == cuentaDestino)
                    {
                        Saldo -= cantidad;
                        cuenta.Depositar(cantidad);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
