import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  form!: FormGroup;

  constructor(private fb: FormBuilder) { }

  get f(): any { return this.form.controls; }

  ngOnInit(): void {
    this.validation();
  }

  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.fb.group({
      titulo: [''],
      primeiroNome: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      ultimoNome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', Validators.required],
      funcao: ['', Validators.required],
      password: ['', [Validators.minLength(6) , Validators.nullValidator]], //Verifica se o campo é nulo, se não for, ele passa o minimo de 6 caracteres
      confirmePassword: ['', Validators.nullValidator],
    }, formOptions);
  };

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }
  }

  public resetForm(event : any): void {
    event.preventDefault();
    this.form.reset();
    this.form.get('titulo')?.setValue('');
  }

}


