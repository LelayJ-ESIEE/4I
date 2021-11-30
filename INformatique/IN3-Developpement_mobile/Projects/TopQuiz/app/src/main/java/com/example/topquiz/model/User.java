package com.example.topquiz.model;

import android.os.Parcel;
import android.os.Parcelable;

public class User implements Parcelable {
    private String mFirstName;
    private int mScore;


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

    protected User(Parcel in) {
        mFirstName = in.readString();
        mScore = in.readInt();
    }

    public User(String firstName, int score) {
        mFirstName = firstName;
        mScore = score;
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeString(mFirstName);
        dest.writeInt(mScore);
    }

    public String getFirstName() {
        return mFirstName;
    }

    public int getScore() {
        return mScore;
    }

    public void setFirstName(String mFirstName) {
        this.mFirstName = mFirstName;
    }

    public void setScore(int score) {
        mScore = score;
    }

    public void increaseScore(int amount){
        mScore += amount;
    }
}
