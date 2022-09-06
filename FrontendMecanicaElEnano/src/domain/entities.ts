export interface Vehiculo
{
    patente: string,
    cliente: string,
    marcaYModelo: string,
    direccion: string,
    telefono: string,
    mail: string,
    nroMotor: string,
    nroChasis: string,
    cuit: string
}

export enum state
{
    viewing,
    creating,
    updating,
}