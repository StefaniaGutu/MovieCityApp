import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MovieDetailsPageDto } from '../interfaces/movie-details-page-dto';
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

  getMovieDetails(movieId: string){
    return this.http.get<MovieDetailsPageDto>(this.url+'/Movie/getMovieDetails?id='+movieId);
  }

  addLike(movieId: string, isLiked: boolean){
    return this.http.post<any>(this.url+'/WatchAndLike/addLike?movieId='+movieId+'&isLiked='+isLiked, null);
  }

  addWatch(movieId: string, isAlreadyWatched: boolean){
    return this.http.post<any>(this.url+'/WatchAndLike/addWatch?movieId='+movieId+'&isAlreadyWatched='+isAlreadyWatched, null);
  }

  getWatchlist(){
    return this.http.get<MovieWithDetailsDto[]>(this.url+'/Movie/getWatchlist');
  }

  getLikedMovies(){
    return this.http.get<MovieWithDetailsDto[]>(this.url+'/Movie/getLikedMovies');
  }

  getWatchedMovies(){
    return this.http.get<MovieWithDetailsDto[]>(this.url+'/Movie/getWatchedMovies');
  }
}
