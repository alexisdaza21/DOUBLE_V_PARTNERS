
import { Length, IsEmail, IsOptional } from 'class-validator';

export class Usuario {
    id?: number;

    @IsEmail({}, { message: 'Correo no tiene un formato valido' })
    @Length(3, 100, { message: 'Correo debe tener entre 3 y 100 caracteres' })
    email?: string;


    @IsOptional()
    @Length(6, 20, { message: 'La contraseña debe tener entre 6 y 20 caracteres' })
    password?: string;


}
