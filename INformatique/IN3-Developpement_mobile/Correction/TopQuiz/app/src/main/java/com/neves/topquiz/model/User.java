package com.neves.topquiz.model;

import android.os.Parcel;
import android.os.Parcelable;

public class User implements Parcelable {

    private String mFirstName;
    private int score;

    public static final Creator<User> CREATOR = new Creator<User>() {
        @Override
        public User createFromParcel(Parcel in) {
            return new User(in);
        }

        @Override
        public User[] newArray(int size) {
            return new User[size];
        }
    };

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel out, int flags) {
        out.writeInt(score);
        out.writeString(mFirstName);
    }

    private User(Parcel in) {
        score = in.readInt();
        mFirstName = in.readString();
    }

    /**
     * Permet d'incrémenter de 1 le score
     */
    public void incScoreByOne(){
        score++;
    }

    /**
     * Retourne le score de l'utilisateur
     * @return Le score de l'utilisateur
     */
    public int getScore() {
        return score;
    }

    /**
     * Affecte un score à un utilisateur
     * @param score de l'utilisateur
     */
    public void setScore(int score) {
        this.score = score;
    }

    /**
     * Constructeur
     * @param mFirstName : est le nom de l'utilisateur
     * @param score : esr le score du joueur
     */
    public User(String mFirstName, int score) {
        this.mFirstName = mFirstName;
        this.score = score;
    }

    /**
     * Permet de récupérer le nom de l'utilisateur
     * @return : le nom de l'utilisateur
     */
    public String getFirstName() {
        return mFirstName;
    }

    /**
     * Permet d'ajouter un nom à utilisateur
     * @param firstName : est le nom de l'utilisateur
     */
    public void setFirstName(String firstName) {
        mFirstName = firstName;
    }
}
