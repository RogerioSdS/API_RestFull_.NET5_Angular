/* tslint:disable:no-unused-variable */

import { TestBed, async } from '@angular/core/testing';
import { DateTimeFormatPipe } from './DateTimeFormat.pipe';

describe('Pipe: DateTimeFormate', () => {
  it('create an instance', () => {
    let pipe = new DateTimeFormatPipe('Esse classe utilza somente em testes');
    expect(pipe).toBeTruthy();
  });
});
