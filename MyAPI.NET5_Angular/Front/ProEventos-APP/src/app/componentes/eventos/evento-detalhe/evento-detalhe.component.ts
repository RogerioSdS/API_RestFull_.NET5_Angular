import { Component, OnInit } from '@angular/core';
import {
  Form,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  form: FormGroup;

  get f(): any {
    /*quando for utilizar o form, o f é o nome do form, ele ira retornar os controles do form que foram inserido no metodo validation */
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    };
  }

  constructor(private fb: FormBuilder, private localeService: BsLocaleService) {
    this.localeService.use('pt-BR');
  }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {
    /**
     * Este método cria um formulário de validação para um evento. Ele recebe o
     * formBuilder como parâmetro e retorna um objeto do tipo FormGroup. O
     * formGroup é um objeto que contém os campos do formulário e as respectivas
     * validações.
     */
    this.form = this.fb.group({
      // Tema do evento (obrigatório, mínimo 4 caracteres, máximo 50)
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],

      // Data do evento (obrigatório)
      dataEvento: ['', Validators.required],

      // Local do evento (obrigatório)
      local: ['', Validators.required],

      // Quantidade de pessoas (obrigatório, máximo 120.000)
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],

      // Telefone (obrigatório)
      telefone: ['', Validators.required],

      // E-mail (obrigatório, deve ser um e-mail válido)
      email: ['', [Validators.required, Validators.email]],

      // URL da imagem do evento (obrigatório)
      imagemURL: ['', Validators.required],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoFormat: FormControl): any {
    return { 'is-invalid': campoFormat.invalid && campoFormat.touched };
  }
}
