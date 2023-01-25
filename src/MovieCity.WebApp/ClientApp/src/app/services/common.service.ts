import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  private updateHeader = new Subject<any>();

  constructor() { }

  sendUpdate(message: any) { //the component that wants to update something, calls this fn
    this.updateHeader.next(message); //next() will feed the value in Subject
}

getUpdate(): Observable<any> { //the receiver component calls this function 
    return this.updateHeader.asObservable(); //it returns as an observable to which the receiver funtion will subscribe
}
}
