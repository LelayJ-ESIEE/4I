package com.neves.topquiz.controller;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.neves.topquiz.R;
import com.neves.topquiz.model.User;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {
    public static final int GAME_ACTIVITY_REQUEST_CODE = 42;
    private TextView mGreetingText;
    private EditText mNameInput;
    private Button mPlayButton;
    private User mUser;
    private List<User> users;
    private SharedPreferences mPreferences;

    public static final String USER = "USER";
    public static final String USERS = "USERS";


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        this.configureToolbar();

        System.out.println("MainActivity::onCreate()");

        mUser = new User("", 0);
        users = new ArrayList<User>();
        mPreferences = getPreferences(MODE_PRIVATE);
        mGreetingText = (TextView) findViewById(R.id.activity_main_greeting_txt);
        mNameInput = (EditText) findViewById(R.id.activity_main_name_input);
        mPlayButton = (Button) findViewById(R.id.activity_main_play_btn);
        mPlayButton.setEnabled(false);

        greetUser();

        mNameInput.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                mPlayButton.setEnabled(s.toString().length() != 0);
                if (s.toString().length() != 0) {
                    mPlayButton.setBackgroundColor(getColor(R.color.colorGreen));
                    String fulltext = getString(R.string.welcome) + mNameInput.getText().toString()
                            + getString(R.string.lastScore) + "0"
                            + getString(R.string.tryAgain);
                    mGreetingText.setText(fulltext);
                } else {
                    mPlayButton.setBackgroundColor(getColor(R.color.colorRed));
                    String fulltext = getString(R.string.welcome) + "\n" + getString(R.string.enterPseudo);
                    mGreetingText.setText(fulltext);
                }
                for (int i = 0; i < users.size(); i++) {
                    if (mNameInput.getText().toString().equals(users.get(i).getFirstName())) {
                        String fulltext = getString(R.string.welcome) + users.get(i).getFirstName()
                                + getString(R.string.lastScore) + users.get(i).getScore()
                                + getString(R.string.tryAgain);
                        mGreetingText.setText(fulltext);
                    }
                }
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });

        mPlayButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mUser.setFirstName(mNameInput.getText().toString());
                Intent gameActivity = new Intent(MainActivity.this, GameActivity.class);
                gameActivity.putExtra(USER, mUser);
                startActivityForResult(gameActivity, GAME_ACTIVITY_REQUEST_CODE);
            }
        });
    }

    /**
     * Instancie notre menu personnalisé (menu_general.xml) et remplace celui contenu par
     * activity_main.xml
     *
     * @param menu
     * @return
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_general, menu);
        return true;
    }

    /**
     * Ajoute la toolbar déclarée dans activity_main.xml à notre vue
     */
    private void configureToolbar() {
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
    }

    /**
     * Défini l'action des différents "boutons" de la toolBar lors d'un clique
     *
     * @param item
     * @return
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.menu_activity_main_drapeau:
                Toast.makeText(this, "changement de langue", Toast.LENGTH_LONG).show();
                return true;
            case R.id.menu_activity_main_parametres:
                Toast.makeText(this, "afficher vue paramètre", Toast.LENGTH_LONG).show();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        if (GAME_ACTIVITY_REQUEST_CODE == requestCode && RESULT_OK == resultCode) {
            mUser = data.getParcelableExtra(GameActivity.USER);
            Gson gson = new Gson();
            mPreferences.edit().putString(USER, gson.toJson(mUser)).apply();
            mPreferences.edit().putString(USERS, gson.toJson(users)).apply();
            greetUser();
        }
    }

    /**
     * Récupère les informations d'un utilisateur ayant précédément utilisé l'application et
     * change l'interface au besoin
     */
    private void greetUser() {
        //mPreferences.edit().clear().apply();
        if (mPreferences.contains(USERS)) {
            int flag = 0;
            int i = 0;
            Gson gson = new Gson();
            String user = mPreferences.getString(USERS, null);

            if (user != null) {
                users = gson.fromJson(user, new TypeToken<List<User>>() {
                }.getType());
            }

            for (i = 0; i < users.size(); i++) {
                if (users.get(i).getFirstName().equals(mUser.getFirstName()) && !(mUser.getFirstName().equals(""))) {
                    if (users.get(i).getScore() < mUser.getScore()) {
                        users.get(i).setScore(mUser.getScore());
                    } else {
                        mUser = users.get(i);
                        mPreferences.edit().putString(USER, gson.toJson(mUser)).apply();
                    }
                    flag = 1;
                }
                //Log.d("test", "User : " + users.get(i).getFirstName() + " score : " + users.get(i).getScore());
            }
            if (flag == 0 && !(mUser.getFirstName().equals(""))) {
                users.add(mUser);
                mPreferences.edit().putString(USERS, gson.toJson(users)).apply();
            }

            //Log.d("test", "Taille users : " + users.size());

            user = mPreferences.getString(USER, "");

            if (user != null) {
                mUser = gson.fromJson(user, User.class);
            }
            if (mUser.getFirstName() != null) {
                String fulltext = getString(R.string.welcome) + mUser.getFirstName()
                        + getString(R.string.lastScore) + mUser.getScore()
                        + getString(R.string.tryAgain);
                mGreetingText.setText(fulltext);
                mNameInput.setText(mUser.getFirstName());
                mNameInput.setSelection(mUser.getFirstName().length());
                mPlayButton.setEnabled(true);
                mPlayButton.setBackgroundColor(getColor(R.color.colorGreen));
            }
        } else {
            //Log.d("test", "USER vide");
        }
    }

    @Override
    protected void onStart() {
        super.onStart();
        System.out.println("MainActivity::onStart()");
    }

    @Override
    protected void onResume() {
        super.onResume();
        System.out.println("MainActivity::onResume()");
    }

    @Override
    protected void onPause() {
        super.onPause();
        System.out.println("MainActivity::onPause()");
    }

    @Override
    protected void onStop() {
        super.onStop();
        System.out.println("MainActivity::onStop()");
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        System.out.println("MainActivity::onDestroy()");
    }
}
