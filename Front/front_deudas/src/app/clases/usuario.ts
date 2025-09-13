
import { Length, IsEmail, IsOptional } from 'class-validator';

export class Usuario {
    @IsEmail({}, { message: 'Correo no es válido' })
    @Length(3, 100, { message: 'El usuario debe tener entre 3 y 100 caracteres' })
    correo?: string;


    @IsOptional()
    @Length(6, 20, { message: 'La contraseña debe tener entre 6 y 20 caracteres' })
    password?: string;

    // constructor(data?: Partial<Usuario>) {
    //     if (data) Object.assign(this, data);
    // }
}
