import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ng2-cookies';
import { environment } from 'src/environments/environment';
import { LoginDTO } from '../interfaces/login-dto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  url:string = environment.baseUrl;

  constructor(private http:HttpClient, private cookieService: CookieService, private router:Router) { }

  loginUser(user:LoginDTO){
    return this.http.post<any>(this.url+"/UserAccount/login", user);
  }

  registerUser(user:any){
    return this.http.post<any>(this.url+"/UserAccount/register", user);
  }

  saveLoggedUser(loggedUser:any){
    this.cookieService.set('username', loggedUser.username);
    this.cookieService.set('isAuthenticated', 'true');
    this.cookieService.set('token', loggedUser.token);

    if(loggedUser.image != null){
      this.cookieService.set('image', loggedUser.image);
    }

    // GlobalComponent.isAuthenticated = true;
    // GlobalComponent.token = loggedUser.token;
    // GlobalComponent.username = loggedUser.username;
  }

  isUserLoggedIn(){
    return this.cookieService.get('isAuthenticated') === 'true';
  }

  getCurrentUserImage(){
    return this.cookieService.get('image');
  }

  getCurrentUserUsername(){
    return this.cookieService.get('username');
  }

  logoutUser(){
    this.cookieService.deleteAll();
    this.cookieService.set("isAuthenticated", 'false');
    this.router.navigate(['/login']); 

    // GlobalComponent.isAuthenticated = false;
    // GlobalComponent.token = '';
    // GlobalComponent.username = '';
  }
}
