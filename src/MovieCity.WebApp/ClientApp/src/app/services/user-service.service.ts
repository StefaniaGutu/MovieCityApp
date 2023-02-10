import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserProfileDto } from '../interfaces/user-profile-dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  url:string = environment.baseUrl;
  
  constructor(private http:HttpClient) { }

  getUserProfile(movieId: string){
    return this.http.get<UserProfileDto>(this.url+'/UserAccount/profile?id='+movieId);
  }

}
