import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CadastroService {

  constructor(private http: HttpClient) { }

  
  async setUsuario(form:Object):Promise<any>{
    console.log(form);
    debugger;
    const options: Object = new HttpHeaders({
      'Content-Type' : "application/json"
    });
    return this.http.post('https://localhost:7031/api/usuario',form,options).toPromise();
  }
}
