import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import { Constants } from '../util/constants';


@Pipe({
  name: 'DateFormatPipe'
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {
  transform(value: any, args?: any): any {
    const dateMoment: moment.Moment = moment(value, 'DD/MM/YYYY hh:mm:ss');
    const dateJS = dateMoment.toDate();
    return super.transform(dateJS, Constants.DATE_TIME_FMT);
  }

}
