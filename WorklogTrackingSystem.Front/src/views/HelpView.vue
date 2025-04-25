<template>
  <div class="help-page">
    <header class="help-page__header">
      <div class="content">
        <h1>Help Page</h1>
        <RouterLink to="/login" class="back-to-login">< Back to login</RouterLink>
      </div>
    </header>

    <div class="help-page__main-content">
      Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi eget est sed metus dictum
      varius quis sed dolor. Donec et imperdiet sapien. Duis sed mi lectus. Donec sagittis sagittis
      purus, nec elementum ipsum tristique non. Proin eget orci a leo aliquam placerat ut vitae
      mauris. Vestibulum sodales lectus non odio lacinia, eu dictum orci pulvinar. Duis consectetur
      tellus ac interdum fermentum. Cras sapien justo, placerat ut est eu, dapibus finibus lectus.
      Sed vitae faucibus quam, congue accumsan tortor. Aenean tristique ultricies velit, egestas
      tincidunt nulla. Suspendisse consectetur erat tortor, vel luctus erat laoreet sed. Morbi
      suscipit orci sit amet maximus varius. Aliquam ex nisi, euismod a libero eu, facilisis
      molestie lectus. Integer non magna mollis, venenatis nisl at, dictum urna. Nunc fermentum
      magna erat, at condimentum libero pellentesque ut. Morbi sodales lectus eget tortor ultricies,
      eget scelerisque tellus pulvinar. Vestibulum semper finibus tortor, sit amet pharetra metus
      iaculis non. Maecenas bibendum finibus metus eget tristique. In ut nisl id ante sollicitudin
      auctor. Vivamus non rutrum ex. In molestie malesuada bibendum. Aliquam erat volutpat. Cras
      commodo nisi vel enim blandit, vel maximus neque posuere. Nam fringilla lacinia augue in
      rutrum. Donec fringilla ex vitae turpis feugiat, a fringilla lorem luctus. Fusce id quam
      nulla. Proin malesuada tellus quis urna dapibus posuere. Morbi et erat condimentum metus
      semper pretium. Suspendisse lobortis urna nisi, gravida sollicitudin nibh facilisis a.
      Curabitur quis sagittis felis. Cras in arcu tincidunt, sollicitudin felis eget, pellentesque
      est. Cras leo purus, dignissim non dictum sit amet, mattis et arcu. Nunc in varius erat. Nam
      congue non est non laoreet. Maecenas quis hendrerit est. Integer eleifend purus turpis, in
      gravida mauris convallis a. Sed at nisi at quam commodo viverra. Aenean mollis, arcu non
      pretium pulvinar, mi nibh faucibus purus, eget euismod nunc augue sit amet mi. Quisque at urna
      a sem luctus fringilla ac a lorem. Donec non odio mollis, eleifend magna eu, aliquet purus.
      Proin aliquam sagittis purus non placerat. Nullam fringilla, erat et tempor vestibulum, felis
      mi pellentesque neque, porta tristique leo nunc sed neque. Vivamus varius ultrices lacus, in
      lobortis nisi ultricies in. Phasellus risus felis, eleifend sed erat id, scelerisque euismod
      eros. Nullam fringilla risus nec sem scelerisque, vitae cursus quam lacinia. In vulputate
      rutrum arcu nec feugiat. Praesent facilisis leo at est scelerisque pharetra. Suspendisse felis
      est, commodo vitae ligula quis, euismod pharetra nunc. Quisque pretium nec dolor eget laoreet.
      Aenean scelerisque vehicula ex, eget tristique justo laoreet eu. Etiam ex nibh, sagittis et
      nibh sit amet, efficitur dignissim enim. Etiam ut iaculis nisl. Vestibulum placerat risus
      lacus, sit amet ultrices dolor porta id.
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
const router = useRouter()
</script>

<style lang="scss" scoped>
.help-page {
  display: flex;
  flex-direction: column;
  height: 100vh;

  &__header {
    background-color: #2c3e50;
    color: white;
    padding: 1rem;

    .content {
      display: flex;
      flex-flow: column;
      align-items: flex-start;
      max-width: 1200px;
      margin: 0 auto;

      .back-to-login {
        color: white;
        text-decoration: none;
        font-size: 0.875rem;
        font-weight: bold;

        &:hover {
          text-decoration: underline;
        }
      }
    }
  }

  &__main-content {
    padding: 1rem;
    background-color: #fafafa;
    flex: 1;
  }
}
</style>
