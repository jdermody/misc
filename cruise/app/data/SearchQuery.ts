import { Movie } from "./Models";
import { List } from "immutable";

export class SearchQuery {
    readonly query: string;
    readonly regex: RegExp;

    constructor(query: string) {
        this.query = query;
        this.regex = new RegExp(query, "gi");
    }

    search(movies: List<Movie>) {
        const {regex, query} = this;
        const hasQuery = query && query.trim().length ? true : false;
        if(!hasQuery)
            return movies;

        return movies.filter(m => 
            regex.test(m.title)
            || regex.test(m.overview)
        );
    }
}