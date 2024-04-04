<template>
    <form onsubmit="return false;" action="#">
        <table class="form-table">
            <tr>
                <td><label class="star" for="title">Title</label></td>
                <td><input id="title" name="title" type="text" placeholder="The main frame" value="Main" minlength="5"
                        required></td>
            </tr>

            <tr>
                <td><label class="star" for="background">Background</label></td>
                <td>
                    <div class="part-container">
                        <input type="list" id="background-saved" name="background-saved" placeholder="Select old one..."
                            autocomplete="off" list="background-list">
                        <datalist id="background-list" class="image-list">
                            <option>MyBackground.png</option>
                        </datalist>
                        <input id="background" name="background" type="file" accept="image/png">
                    </div>
                </td>
            </tr>

            <tr>
                <td><label for="background-music">Background music</label></td>
                <td>
                    <div class="part-container">
                        <input type="list" id="music-saved" name="music-saved" placeholder="Select old one..."
                            autocomplete="off" list="music-list">
                        <datalist id="music-list" class="music-list">
                            <option>MySound.mp3</option>
                        </datalist>
                        <input id="background-music" name="background-music" type="file" accept="audio/mp3">
                    </div>
                </td>
            </tr>

            <tr>
                <td><label class="star" for="text">Text</label></td>
                <td>
                    <textarea id="text" placeholder="The story I was told..." name="text" required></textarea>
                </td>
            </tr>

            <tr>
                <td><label for="background-music">Answers</label></td>
                <td>
                    <div>
                        <img id="answer-add" src="@/assets/images/stories/plus.png" class="add-icon answer-icon"
                            title="Add new answer">
                        <div id="answer-list" class="answer-list">

                        </div>
                    </div>
                </td>
            </tr>

            <tr>
                <td><label for="left-image">Left image</label></td>
                <td>
                    <div class="part-container">
                        <input type="list" id="left-saved" name="left-saved" placeholder="Select old one..."
                            autocomplete="off" list="left-list">
                        <datalist id="left-list" class="image-list">
                            <option>Left.png</option>
                        </datalist>
                        <input id="left-image" name="left-image" type="file" accept="image/png">
                    </div>
                </td>
            </tr>

            <tr>
                <td><label for="under-image">Center under image</label></td>
                <td>
                    <div class="part-container">
                        <input type="list" id="under-saved" name="under-saved" placeholder="Select old one..."
                            autocomplete="off" list="under-list">
                        <datalist id="under-list" class="image-list">
                            <option>Under.png</option>
                        </datalist>
                        <input id="under-image" name="under-image" type="file" accept="image/png">
                    </div>
                </td>
            </tr>

            <tr>
                <td><label for="over-image">Center over image</label></td>
                <td>
                    <div class="part-container">
                        <input type="list" id="over-saved" name="over-saved" placeholder="Select old one..."
                            autocomplete="off" list="over-list">
                        <datalist id="over-list" class="image-list">
                            <option>Over.png</option>
                        </datalist>
                        <input id="over-image" name="over-image" type="file" accept="image/png">
                    </div>
                </td>
            </tr>

            <tr>
                <td><label for="right-image">Right image</label></td>
                <td>
                    <div class="part-container">
                        <input type="list" id="right-saved" name="right-saved" placeholder="Select old one..."
                            autocomplete="off" list="right-list">
                        <datalist id="right-list" class="image-list">
                            <option>Right.png</option>
                        </datalist>
                        <input id="right-image" name="right-image" type="file" accept="image/png">
                    </div>
                </td>
            </tr>

            <tr>
                <td><label for="auto-save">Auto-save on frame changing</label></td>
                <td><input id="auto-save" name="auto-save" type="checkbox" checked></td>
            </tr>

            <tr>
                <td></td>
                <td>All mandatory fields are marked by <label class="star"></label></td>
            </tr>
            <tr>
                <td><input type="submit" value="Save" style="margin-top: 20px;"></td>
                <td></td>
            </tr>
        </table>

    </form>
</template>