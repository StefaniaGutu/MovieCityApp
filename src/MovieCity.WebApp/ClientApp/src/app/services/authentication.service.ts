import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LoginDTO } from '../interfaces/login-dto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  url:string = environment.baseUrl;

  constructor(private http:HttpClient) { }

  loginUser(user:LoginDTO){
    return this.http.post<any>(this.url+"/UserAccount/login", user);
  }

  registerUser(user:any){
    return this.http.post<any>(this.url+"/UserAccount/register", user);
  }
}
