import { Component, ElementRef, ViewChild, ChangeDetectorRef } from '@angular/core';
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl, Validators, AbstractControl, FormArray, FormBuilder, ValidatorFn } from '@angular/forms';
import * as moment from 'moment';
import { cpf } from 'cpf-cnpj-validator';
import { ToastrService } from 'ngx-toastr';

import { CadastroService } from '../cadastro.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-formulario',
  templateUrl: './formulario.component.html',
  styleUrls: ['./formulario.component.css']
})
export class FormularioComponent {
  ngForm!: FormGroup;
  mouseOver: boolean;
  isPending:boolean;
  PretensaoSalarial:string;

  @ViewChild("botaoEnvio",{static: false}) botaoEnvio!: ElementRef;
  @ViewChild("formulario",{static: false}) formulario!: ElementRef;
  
  constructor(private toastr: ToastrService, private cadastro: CadastroService, private fb: FormBuilder,
    private change: ChangeDetectorRef){
    this.mouseOver = false; 
    this.isPending = false;
    this.PretensaoSalarial = "";
  }
  
  ngOnInit():void{
    this.ngForm = this.fb.group({
      nome: new FormControl('',{validators: this.validaNome, updateOn: "blur"}),
      senha: new FormControl('',Validators.required),
      csenha: new FormControl('',Validators.required),
      cpf: new FormControl('',this.validarCPF),
      dtnascimento: new FormControl('',{validators: this.validarDataNascimento,updateOn: "blur"}),
      usuario: new FormControl('',{validators:this.validarUsuario,updateOn: "blur"}),
      pretensaoSalarial: new FormControl('',Validators.required),
      escolaridade: new FormControl('MI'),
      sexo: new FormControl('F'),
      ecivil: new FormControl('S'),
      cursos: this.fb.array([this.criarFormCursos()]),
      experiencia: this.fb.array([this.criarFormExperiencia()])
    });
  }

  criarFormCursos(): FormGroup{
    const validaAnos: ValidatorFn = (campo:AbstractControl): {[key:string]:string} | null=>{
      const ano1 = campo.get('inicio_curso')?.value;
      const ano2 = campo.get('fim_curso')?.value;
      if(ano1 && ano2 && (ano1>ano2))
        return {ano2menor: "true"};
      return null;
    }

    return this.fb.group({
      curso: new FormControl('',Validators.required),
      instituicao: new FormControl('',Validators.required),
      inicio_curso: new FormControl('',Validators.required),
      fim_curso: new FormControl('',Validators.required)
    },{validators: validaAnos});
  }

  criarFormExperiencia(){
    const validaAnos: ValidatorFn = (campo:AbstractControl): {[key:string]:string} | null =>{
      const campo1 = campo.get('inicioex')?.value;
      const campo2 = campo.get('fimex')?.value;
      
      if(campo1 && campo2){
        const data1 = moment(campo1);
        const data2 = moment(campo2);

        if(data2.isBefore(data1)) return {"Data1Depois" : "true"}
      }

      return null;
    }

    let exp:FormGroup = this.fb.group({
      empresa: new FormControl('',Validators.required),
      cargo: new FormControl('',Validators.required),
      inicioex: new FormControl('',Validators.required),
      fimex: new FormControl('',Validators.required)
    },{validators:validaAnos});
    return exp;
  }

  isPositive(event:any){
    return event === '-' ? false : true;
  }

  ValorReal(sal:any){
    this.PretensaoSalarial = sal.target.value;
  }

  getArray(array: string){
    return this.ngForm.get(array) as FormArray;
  }

  adicionarFormArray(array:string){
    if(array=="cursos"){
      this.getArray(array).push(this.criarFormCursos());
      return;
    }
    this.getArray(array).push(this.criarFormExperiencia());
    this.change.detectChanges();
  }

  removerLinha(index: number,array: string){
    const ref = this.getArray(array);
    if(ref.length==1) return;
    ref.removeAt(index);
  }

  validaNome(campo: AbstractControl){
    const func: ValidatorFn = (campo:AbstractControl): {[key:string] : any} | null =>{
      const padrao: RegExp = /^[a-zA-Z\s]*$/;
      
      if(campo.value=="") return {Vazio: true};
      if(campo.value && !(padrao.test(campo.value))) return {Alfanumerico: true};
      return null;
    }

    return func(campo);
  }

  validarUsuario(usuario:AbstractControl){
    if(usuario.value=="") return {'vazio':true};
    return Number.isNaN(parseInt(usuario.value)) ? null : {'numerico':true}
  }

  validarCPF(campo:AbstractControl){
    let valido = cpf.isValid(campo.value);
    return valido ? null : {"invalid":true};
  }

  validarDataNascimento(data:AbstractControl){
    let dataNascimento: moment.Moment = moment.utc(data.value);
    let agora: moment.Moment = moment.utc(moment.now());
    let resultado: moment.Moment = agora.add(-18,'year');

    return dataNascimento.isBefore(resultado) ? null : {"menorIdade":true};
  }

  mensagemErro(campo:AbstractControl, chave:string):string{
    if(campo.valid) {return "";}
    if(chave=="experiencia") return "Experiência(s) inválida(s)<br>";
    if(chave=="cursos") return "Certificações inválidas<br>";
    const labels = this.formulario.nativeElement.querySelectorAll("label");
    let label: string = "";
    
    for(let i = 0;i<labels.length;i++){
      if(labels[i].getAttribute("for")==chave){ label = labels[i].innerText; break;}
    }
    return "Campo inválido: " + label.slice(0,label.length-2) + "<br>";
  }

  corMouseOver(){
    if(this.ngForm.pending) return;
    this.mouseOver = true;
  }

  corMouseOut(){
    if(this.ngForm.pending) return;
    this.mouseOver = false;
  }

  corPending(estado:string){
    const myDiv = this.botaoEnvio.nativeElement;

    if(estado=="pending"){
      myDiv.classList.remove("bg-info","bg-primary");
      myDiv.classList.add("bg-secondary");
      return;
    }

    myDiv.classList.add("bg-primary");
    myDiv.classList.remove("bg-secondary");
  }

  async CadastroApi():Promise<any>{
    try{
      let usuario = this.ngForm.value;
      usuario.pretensaoSalarial = parseFloat(this.PretensaoSalarial.replace(",","."));
      let dados = await this.cadastro.setUsuario(usuario);
      console.log(dados);
      
      this.toastr.success("Cadastro realizado com sucesso!");
    }catch(ex){
      console.log(ex);
      this.toastr.error("Cadastro não realizado. Favor tentar novamente.");
      this.corPending("unpending");

      this.isPending = false;
    }
  }

  onSubmit(formulario:FormGroup):void {
    if(!formulario.valid){
      let mensagem: string = "";
      for(let key in formulario.controls){
        formulario.controls[key].markAsTouched();
        mensagem += this.mensagemErro(formulario.controls[key],key);
      }
      this.toastr.error(mensagem);
      return;
    } 
    
    this.isPending = true;
    this.corPending("pending");

    this.CadastroApi();
  }
}
