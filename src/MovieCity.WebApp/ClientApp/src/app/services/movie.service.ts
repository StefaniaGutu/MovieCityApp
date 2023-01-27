import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MovieWithDetailsDto } from '../interfaces/movie-with-details-dto';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  url:string = environment.baseUrl;
  
  constructor(private http:HttpClient) { }

  getAllMoviesAndSeries(){
    return this.http.get<MovieWithDetailsDto[]>(this.url+'/Movie/getAllMovies');
  }
}
