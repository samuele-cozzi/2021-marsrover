import { Pipe, PipeTransform } from '@angular/core';
import { RoverPosition} from '../rover';
/*
 * Raise the value exponentially
 * Takes an exponent argument that defaults to 1.
 * Usage:
 *   value | translate:exponent
 * Example:
 *   {{ 2 | translate:10 }}
 *   formats to: 1024
*/
@Pipe({name: 'translate'})
export class Translate implements PipeTransform {
  transform(values: RoverPosition[], factor = 1): number[] {
    var result: number[] = [];
    values.forEach(element => {
        var x = (element.longitude + 180) * factor;
        var y = (-1 * element.latitude + 90) * factor;
        result.push(x, y);
    });
    return result;
  }
}