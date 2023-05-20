import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConversorService {

  constructor() { }

  converteRespostaUsuario(form:any,pretensalarial:string):Object{
    const usuario = {
      "nome": form.nome,
      "cpf": form.cpf,
      "acesso": {
        "usuario": form.usuario,
        "senha": form.senha
      },
      "datanascimento": form.dtnascimento,
      "pretensaoSalarial": parseFloat(pretensalarial),
      "sexo": form.sexo,
      "escolaridade": form.escolaridade,
      "ecivil": form.ecivil,
      "experiencia": form.experiencia,
      "cursos": form.cursos
    }
    return usuario;
  }
}
